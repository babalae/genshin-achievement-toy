using GenshinAchievement.Core;
using GenshinAchievement.Forms.Hotkey;
using GenshinAchievement.Model;
using GenshinAchievement.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Windows.Media.Ocr;

namespace GenshinAchievement
{
    public partial class FormMain : Form
    {

        private ImageCapture capture = new ImageCapture();

        private YuanShenWindow window = new YuanShenWindow();

        private string thisVersion;

        int x, y, w, h;
        string userDataPath, imgPagePath, imgSectionPath;
        bool stopFlag = false;
        Config config;

        PaimonMoeJson paimonMoeJson = PaimonMoeJson.Builder();

        public FormMain()
        {
            InitializeComponent();
        }

        private bool YSStatus()
        {
            if (window.FindYSHandle())
            {
                lblYSStatus.ForeColor = Color.Green;
                lblYSStatus.Text = "已启动";
                return true;
            }
            else
            {
                lblYSStatus.ForeColor = Color.Red;
                lblYSStatus.Text = "未启动";
                return false;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 标题加上版本号
            string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (currentVersion.Length > 3)
            {
                thisVersion = currentVersion.Substring(0, 3);
                currentVersion = " v" + thisVersion;
            }
            this.Text += currentVersion;
            GAHelper.Instance.RequestPageView($"/achi/main/{thisVersion}", $"进入{thisVersion}版本主界面");

            YSStatus();
            //foreach (var item in paimonMoeJson.All)
            //{
            //    cboEdition.Items.Add(item.Key);
            //}
            cboEdition.Items.Add("天地万象");
            cboEdition.Text = "天地万象";
            userDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
            imgPagePath = Path.Combine(userDataPath, cboEdition.Text + "_img_page");
            imgSectionPath = Path.Combine(userDataPath, cboEdition.Text + "_img_section");


            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (File.Exists(configPath))
            {
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    config = serializer.Deserialize<Config>(File.ReadAllText(configPath));
                }
                catch (Exception ex)
                {
                    PrintMsg("配置解析失败:" + ex.Message);
                    MessageBox.Show(ex.Message, "配置解析失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                config = new Config();
            }

            try
            {
                RegisterHotKey(config.GetHotkeyStop());
            }
            catch (Exception ex)
            {
                PrintMsg(ex.Message);
                MessageBox.Show(ex.Message, "热键注册失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private async void btnOCR_Click(object sender, EventArgs e)
        {
            List<OcrAchievement> list = LoadImgSection();
            btnStart.Text = $"文字识别中...";
            await Ocr(list);
            PrintMsg($"文字识别完成");
            btnStart.Text = $"成就匹配中...";
            Matching(list);
            PrintMsg($"成就匹配完成");
        }

        private void btnAutoArea_Click(object sender, EventArgs e)
        {
            if (!YSStatus())
            {
                PrintMsg("未找到原神进程，请先启动原神！");
                return;
            }

            window.Focus();
            capture.Start();
            Thread.Sleep(500);
            Rectangle rc = window.GetSize();
            x = (int)Math.Ceiling(rc.X * PrimaryScreen.ScaleX);
            y = (int)Math.Ceiling(rc.Y * PrimaryScreen.ScaleY);
            w = (int)Math.Ceiling(rc.Width * PrimaryScreen.ScaleX);
            h = (int)Math.Ceiling(rc.Height * PrimaryScreen.ScaleY);
            Bitmap ysPic = capture.Capture(x, y, w, h);
            Rectangle rect = ImageRecognition.CalculateCatchArea(ysPic);
            //pictureBox1.Image = ysPic;
            //pictureBox1.Image = capture.Capture(x + rect.X, y + rect.Y, rect.Width, rect.Height);
            //InitAreaWindows(rect);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!YSStatus())
                {
                    PrintMsg("未找到原神进程，请先启动原神！");
                    return;
                }
                capture.Start();

                stopFlag = false;
                DisabledAllControl();



                // 1.切换到原神窗口
                PrintMsg($"切换到原神窗口");
                window.Focus();
                Thread.Sleep(200);

                // 2. 定位截图选区
                Rectangle rc = window.GetSize();
                x = (int)Math.Ceiling(rc.X * PrimaryScreen.ScaleX);
                y = (int)Math.Ceiling(rc.Y * PrimaryScreen.ScaleY);
                w = (int)Math.Ceiling(rc.Width * PrimaryScreen.ScaleX);
                h = (int)Math.Ceiling(rc.Height * PrimaryScreen.ScaleY);
                Bitmap ysWindowPic = capture.Capture(x, y, w, h);

                // 使用新的坐标
                Rectangle rect = ImageRecognition.CalculateCatchArea(ysWindowPic);
                PrintMsg($"已定位成就栏位置");
                x += rect.X;
                y += rect.Y;
                w = rect.Width + 2;
                h = rect.Height;

                FormPreviewCaptureArea formPreview = new FormPreviewCaptureArea(capture.Capture(x, y, w, h));
                formPreview.Focus();
                if (formPreview.ShowDialog() != DialogResult.OK)
                {
                    EnabledAllControl();
                    formPreview.Focus();
                    return;
                }
                window.Focus();
                Thread.Sleep(200);

                btnStart.Text = $"按{config.GetHotkeyStop()}终止滚动";
                PrintMsg($"0.5s后开始自动滚动截图，按{config.GetHotkeyStop()}终止滚动！");
                Thread.Sleep(500);
                YSClick();

                IOUtils.CreateFolder(userDataPath);
                IOUtils.CreateFolder(imgPagePath);
                IOUtils.DeleteFolder(imgPagePath);

                paimonMoeJson = PaimonMoeJson.Builder();

                await Task.Run(async () =>
                {
                    // 3. 滚动截图
                    int rowIn = 0, rowOut = 0, n = 0;
                    while (rowIn < 15 && rowOut < 15)
                    {
                        if (stopFlag)
                        {
                            PrintMsg($"滚动已经终止");
                            break;
                        }
                        try
                        {
                            Bitmap pagePic = capture.Capture(x, y, w, h);
                            if (n % config.GetCaptureInterval() == 0)
                            {
                                pagePic.Save(Path.Combine(imgPagePath, n + ".png"));
                                //PrintMsg($"{n}：截图并保存");
                            }

                            Bitmap onePixHightPic = capture.Capture(x, y + h - 20, w, 1); // 截取一个1pix的长条
                            if (ImageRecognition.IsInRow(onePixHightPic))
                            {
                                rowIn++;
                                rowOut = 0;
                            }
                            else
                            {
                                rowIn = 0;
                                rowOut++;
                            }
                        }
                        catch (Exception ex)
                        {
                            PrintMsg(ex.Message + Environment.NewLine + ex.StackTrace);
                        }

                        YSClick();
                        window.MouseWheelDown();
                        n++;
                    }
                    if (!stopFlag)
                    {
                        Bitmap lastPagePic = capture.Capture(x, y, w, h);
                        lastPagePic.Save(Path.Combine(imgPagePath, ++n + ".png"));
                        PrintMsg($"滚动截图完成");

                        // 4. 分割截图
                        btnStart.Text = $"截图处理中...";
                        PageToSection();
                        PrintMsg($"截图处理完成");

                        // 5. OCR
                        List<OcrAchievement> list = LoadImgSection();
                        btnStart.Text = $"文字识别中...";
                        await Ocr(list);
                        PrintMsg($"文字识别完成");
                        btnStart.Text = $"成就匹配中...";
                        Matching(list);
                        PrintMsg($"成就匹配完成");
                        PrintMsg($"你可以点击对应的按钮导出成就啦~");
                    }
                });

                capture.Stop();

                EnabledAllControl();
            }
            catch (Exception ex)
            {
                EnabledAllControl();
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DisabledAllControl()
        {
            cboEdition.Enabled = false;
            btnStart.Enabled = false;
            btnExport1.Enabled = false;
            btnExport2.Enabled = false;
            btnExport3.Enabled = false;
        }
        private void EnabledAllControl()
        {
            cboEdition.Enabled = true;
            btnStart.Enabled = true;
            btnStart.Text = "一键识别成就";
            btnExport1.Enabled = true;
            btnExport2.Enabled = true;
            btnExport3.Enabled = true;
        }

        /// <summary>
        /// 读取截图并切片
        /// </summary>
        private void PageToSection()
        {
            IOUtils.CreateFolder(imgSectionPath);
            IOUtils.DeleteFolder(imgSectionPath);

            DirectoryInfo dir = new DirectoryInfo(imgPagePath);
            FileInfo[] fileInfo = dir.GetFiles();
            progressBar1.Maximum = fileInfo.Length;
            progressBar1.Value = 0;
            foreach (FileInfo item in fileInfo)
            {
                Bitmap imgPage = IOUtils.FromFile(item.FullName);
                List<Bitmap> list = ImageRecognition.Split(imgPage);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Save(Path.Combine(imgSectionPath, item.Name + "_" + i + ".png"));
                }
                //PrintMsg($"{item.Name}切片完成");
                progressBar1.Value++;
            }
        }

        private List<OcrAchievement> LoadImgSection()
        {
            List<OcrAchievement> list = new List<OcrAchievement>();
            DirectoryInfo dir = new DirectoryInfo(imgSectionPath);
            FileInfo[] fileInfo = dir.GetFiles();
            foreach (FileInfo item in fileInfo)
            {
                OcrAchievement achievement = new OcrAchievement();
                achievement.Image = IOUtils.FromFile(item.FullName);
                achievement.ImagePath = item.FullName;
                list.Add(achievement);
            }
            return list;
        }

        private async Task Ocr(List<OcrAchievement> achievementList)
        {
            Windows.Globalization.Language lang = new Windows.Globalization.Language("zh-Hans-CN");
            OcrEngine engine = OcrEngine.TryCreateFromLanguage(lang);
            progressBar1.Maximum = achievementList.Count;
            progressBar1.Value = 0;
            foreach (OcrAchievement a in achievementList)
            {
                string r = await a.Ocr(engine);
                Console.WriteLine(r);
                progressBar1.Value++;
            }
        }

        private void Matching(List<OcrAchievement> achievementList)
        {
            progressBar1.Maximum = achievementList.Count;
            progressBar1.Value = 0;
            foreach (OcrAchievement a in achievementList)
            {
                paimonMoeJson.Matching(cboEdition.Text, a);
                progressBar1.Value++;
            }
        }

        private void YSClick()
        {
            window.MouseMove(x, y + h / 2);
            window.MouseLeftDown();
            window.MouseLeftUp();
        }

        private void PrintMsg(string msg)
        {
            msg = DateTime.Now.ToString("HH:mm:ss") + " " + msg;
            Console.WriteLine(msg);
            rtbConsole.Text += msg + Environment.NewLine;
            this.rtbConsole.SelectionStart = rtbConsole.TextLength;
            this.rtbConsole.ScrollToCaret();
        }

        private void btnExport1_Click(object sender, EventArgs e)
        {
            FormText form = new FormText(TextUtils.GeneratePaimonMoeJS(cboEdition.Text, paimonMoeJson));
            form.ShowDialog();
        }

        private void btnExport2_Click(object sender, EventArgs e)
        {
            FormText form = new FormText(TextUtils.GenerateSeelieMeJS(cboEdition.Text, paimonMoeJson));
            form.ShowDialog();
        }

        private void btnExport3_Click(object sender, EventArgs e)
        {
            FormText form = new FormText(TextUtils.GenerateCocogoatWorkJS(cboEdition.Text, paimonMoeJson));
            form.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/babalae/genshin-achievement-toy");
        }

        #region Hotkey
        private Hotkey hotkey;
        private HotkeyHook hotkeyHook;

        public void RegisterHotKey(string hotkeyStr)
        {
            if (string.IsNullOrEmpty(hotkeyStr))
            {
                UnregisterHotKey();
                return;
            }

            hotkey = new Hotkey(hotkeyStr);

            if (hotkeyHook != null)
            {
                hotkeyHook.Dispose();
            }
            hotkeyHook = new HotkeyHook();
            // register the event that is fired after the key press.
            hotkeyHook.KeyPressed += (sender, eventArgs) =>
            {
                stopFlag = true;
            };
            hotkeyHook.RegisterHotKey(hotkey.ModifierKey, hotkey.Key);
        }

        public void UnregisterHotKey()
        {
            if (hotkeyHook != null)
            {
                hotkeyHook.UnregisterHotKey();
                hotkeyHook.Dispose();
            }
        }
        #endregion

    }
}
