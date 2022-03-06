using GenshinAchievement.Core;
using GenshinAchievement.Model;
using GenshinAchievement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinAchievement
{
    public partial class FormMain : Form
    {

        private ImageCapture capture = new ImageCapture();

        private YuanShenWindow window = new YuanShenWindow();

        int x, y, w, h;

        string userDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");

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
            YSStatus();
            foreach (var item in paimonMoeJson.All)
            {
                cboEdition.Items.Add(item.Key);
            }
            cboEdition.Text = "天地万象";
        }

        private List<OcrAchievement> TestLocalAchievementPic()
        {
            List<OcrAchievement> list = new List<OcrAchievement>();
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(userDataPath, cboEdition.Text + "_img"));
            FileInfo[] fileInfo = dir.GetFiles();
            foreach (FileInfo item in fileInfo)
            {
                OcrAchievement achievement = new OcrAchievement();
                achievement.Image = (Bitmap)Image.FromFile(item.FullName);
                achievement.ImagePath = item.FullName;
                list.Add(achievement);
            }
            return list;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnOCR_Click(object sender, EventArgs e)
        {
            List<OcrAchievement> list = TestLocalAchievementPic();
            OcrUtils.Ocr(list);

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
            pictureBox1.Image = capture.Capture(x + rect.X, y + rect.Y, rect.Width, rect.Height);
            //InitAreaWindows(rect);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (!YSStatus())
            {
                PrintMsg("未找到原神进程，请先启动原神！");
                return;
            }
            cboEdition.Enabled = false;
            IOUtils.CreateFolder(userDataPath);

            window.Focus();
            capture.Start();
            Thread.Sleep(200);

            Rectangle rc = window.GetSize();
            x = (int)Math.Ceiling(rc.X * PrimaryScreen.ScaleX);
            y = (int)Math.Ceiling(rc.Y * PrimaryScreen.ScaleY);
            w = (int)Math.Ceiling(rc.Width * PrimaryScreen.ScaleX);
            h = (int)Math.Ceiling(rc.Height * PrimaryScreen.ScaleY);
            Bitmap ysPic = capture.Capture(x, y, w, h);
            // 使用新的坐标
            Rectangle rect = ImageRecognition.CalculateCatchArea(ysPic);
            x += rect.X;
            y += rect.Y;
            w = rect.Width + 2;
            h = rect.Height;
            pictureBox1.Image = capture.Capture(x, y, w, h);


            Thread.Sleep(500);


            List<OcrAchievement> achievementList = new List<OcrAchievement>();

            int n = 0;
            int preCnt = 0;
            //int zeroNum = 0;
            while (true)
            {
                YSClick();

                Bitmap pagePic = capture.Capture(x, y, w, h);
                pictureBox1.Image = pagePic;
                bool succ = ImageRecognition.Split(pagePic, ref achievementList);
                int pageCount = achievementList.Count - preCnt;
                PrintMsg($"当前{pageCount}个成就");
                preCnt = achievementList.Count;
                //if(pageCount == 0)
                //{
                //    if(zeroNum++ > 5)
                //    {
                //        break;
                //    }
                //}
                while (!succ)
                {
                    PrintMsg($"分割时发现位置不正确，向上滚动");
                    YSClick();
                    window.MouseWheelUp();
                    Thread.Sleep(50);
                    pagePic = capture.Capture(x, y, w, h);
                    pictureBox1.Image = pagePic;
                    succ = ImageRecognition.Split(pagePic, ref achievementList);
                }


                int rowNum = 1, scrollCount = 0;
                bool preOnePixHightPicIsInRow = false, first = false;
                while (rowNum <= pageCount && scrollCount < 10)
                {
                    Bitmap onePixHightPic = capture.Capture(x, y + 15, w, 1); // 截取一个1pix的长条
                    if (!first)
                    {
                        YSClick();
                        window.MouseWheelDown();
                        Thread.Sleep(50);
                    }
                    if (ImageRecognition.IsInRow(onePixHightPic))
                    {
                        //PrintMsg($"在行内");
                        if (!preOnePixHightPicIsInRow)
                        {
                            PrintMsg($"进入第{rowNum}行");
                        }
                        preOnePixHightPicIsInRow = true;
                    }
                    else
                    {
                        //PrintMsg($"在行外");
                        if (preOnePixHightPicIsInRow)
                        {
                            PrintMsg($"离开第{rowNum}行");
                            rowNum++;
                            scrollCount = 0;
                        }
                        preOnePixHightPicIsInRow = false;
                    }
                    scrollCount++;
                }

                if (scrollCount >= 10)
                {
                    PrintMsg($"已经到达底部");
                    break;
                }

                n++;
            }

            string imgPath = Path.Combine(userDataPath, cboEdition.Text + "_img");
            IOUtils.CreateFolder(imgPath);
            IOUtils.DeleteFolder(imgPath);
            foreach (OcrAchievement a in achievementList)
            {
                a.Split();
                a.Image.Save(Path.Combine(imgPath, a.Index + ".png"));
            }
            PrintMsg($"文件写入完成");
            cboEdition.Enabled = true;
        }

        private void YSClick()
        {
            window.MouseMove(x, y + h / 2);
            window.MouseLeftDown();
            window.MouseLeftUp();
        }

        private void PrintMsg(string msg)
        {
            msg = DateTime.Now + " " + msg;
            Console.WriteLine(msg);
            rtbConsole.Text += msg + Environment.NewLine;
            this.rtbConsole.SelectionStart = rtbConsole.TextLength;
            this.rtbConsole.ScrollToCaret();
        }



        private void timerCapture_Tick(object sender, EventArgs e)
        {

        }
    }
}
