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

        private FormMotionArea area;

        private YuanShenWindow window = new YuanShenWindow();

        int x, y, w, h;


        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {


            //TestScreen();
            //using (var ocr = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "chi_sim", EngineMode.Default))
            //{
            //    var pix = PixConverter.ToPix(new Bitmap(@"D:\HuiTask\原神成就导出\测试图片1.png"));
            //    using (var page = ocr.Process(pix))
            //    {
            //        string text = page.GetText();
            //        PrintMsg(text);
            //    }
            //}

            //ImageRecognition.HighlightPic(new Bitmap(@"D:\HuiPrograming\Projects\CSharp\GenshinAchievement\GenshinAchievement\bin\Debug\10.png")).Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "11.png"));
            //ImageRecognition.HighlightBorder(new Bitmap(@"D:\HuiTask\原神成就导出\测试图片1.png")).Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3.png"));

            //List<Achievement> list = ImageRecognition.Split(new Bitmap(@"D:\HuiTask\原神成就导出\测试图片1.png"));
            //foreach (Achievement a in list)
            //{
            //    a.Split();
            //    a.Image.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testout", a.Index + ".png"));

            //}

            //capture.Start();
            //timerCapture.Start();

            //if (window.FindYSHandle())
            //{
            //    capture.Start();
            //    //timerCapture.Start();
            //}

        }

        private List<OcrAchievement> TestLocalAchievementPic()
        {
            List<OcrAchievement> list = new List<OcrAchievement>();
            DirectoryInfo dir = new DirectoryInfo(@"testout");
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

        private void InitAreaWindows(Rectangle rect)
        {
            if (area == null)
            {
                area = new FormMotionArea();
                area.FormMainInstance = this;
            }
            area.Location = new Point(rect.X, rect.Y);
            area.Size = new Size(rect.Width, rect.Height);
            area.Show();

            //if (Properties.Settings.Default.AreaLocation.X >= 0 && Properties.Settings.Default.AreaLocation.Y >= 0)
            //{
            //    area.Location = Properties.Settings.Default.AreaLocation;
            //}
            //area.Size = Properties.Settings.Default.AreaSize;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (area != null)
            {
                Properties.Settings.Default.AreaLocation = area.Location;
                Properties.Settings.Default.AreaSize = area.Size;
                Properties.Settings.Default.Save();
                area.Close();
            }

        }

        private void btnOCR_Click(object sender, EventArgs e)
        {
            List<OcrAchievement> list = TestLocalAchievementPic();
            OcrUtils.Ocr(list);

        }

        private void btnAutoArea_Click(object sender, EventArgs e)
        {
            if (!window.FindYSHandle())
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

            if (!window.FindYSHandle())
            {
                PrintMsg("未找到原神进程，请先启动原神！");
                return;
            }

            window.Focus();
            capture.Start();
            //Thread.Sleep(500);

            //Rectangle rc = window.GetSize();
            //x = (int)Math.Ceiling(rc.X * PrimaryScreen.ScaleX);
            //y = (int)Math.Ceiling(rc.Y * PrimaryScreen.ScaleY);
            //w = (int)Math.Ceiling(rc.Width * PrimaryScreen.ScaleX);
            //h = (int)Math.Ceiling(rc.Height * PrimaryScreen.ScaleY);
            //Bitmap ysPic = capture.Capture(x, y, w, h);
            //// 使用新的坐标
            //Rectangle rect = ImageRecognition.CalculateCatchArea(ysPic);
            //x += rect.X;
            //y += rect.Y;
            //w = rect.Width;
            //h = rect.Height;
            //Bitmap aaa = capture.Capture(x, y, w, h);
            //pictureBox1.Image = aaa;

            x = 601;
            y = 135;
            w = 938;
            h = 1040;

            Thread.Sleep(1000);


            List<OcrAchievement> achievementList = new List<OcrAchievement>();

            int n = 0;
            int preCnt = 0;
            while (true)
            {
                window.MouseMove(x, y + h / 2);
                window.MouseLeftDown();
                window.MouseLeftUp();

                Bitmap pagePic = capture.Capture(x, y, w, h);
                //pictureBox1.Image = pagePic;
                bool succ = ImageRecognition.Split(pagePic, ref achievementList);
                int pageCount = achievementList.Count - preCnt;
                PrintMsg($"当前{pageCount}个成就");
                preCnt = achievementList.Count;
                while (!succ)
                {
                    PrintMsg($"分割时发现位置不正确，向上滚动");
                    ClickAndWheelDown();
                    pagePic = capture.Capture(x, y, w, h);
                    //pictureBox1.Image = pagePic;
                    succ = ImageRecognition.Split(pagePic, ref achievementList);
                }


                int rowNum = 1, scrollCount = 0;
                bool preOnePixHightPicIsInRow = false, first = false;
                while (rowNum <= pageCount && scrollCount < 10)
                {
                    Bitmap onePixHightPic = capture.Capture(x, y, w, 1); // 截取一个1pix的长条
                    if (!first)
                    {
                        ClickAndWheelDown();
                    }
                    if (ImageRecognition.IsInRow(onePixHightPic))
                    {
                        PrintMsg($"在行内");
                        if (!preOnePixHightPicIsInRow)
                        {
                            PrintMsg($"进入第{rowNum}行");
                        }
                        preOnePixHightPicIsInRow = true;
                    }
                    else
                    {
                        PrintMsg($"在行外");
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
                //if (n == 2)
                //{
                //    break;
                //}
            }

            foreach (OcrAchievement a in achievementList)
            {
                a.Split();
                a.Image.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testout", a.Index + ".png"));
            }
            PrintMsg($"文件写入完成");
            //timerCapture.Start();
        }

        private void ClickAndWheelDown()
        {
            window.MouseMove(x, y + h / 2);
            window.MouseLeftDown();
            window.MouseLeftUp();
            window.MouseWheelDown();
            Thread.Sleep(50);
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
            //Rectangle rc = window.GetSize();
            //x = (int)Math.Ceiling(rc.X * PrimaryScreen.ScaleX);
            //y = (int)Math.Ceiling(rc.Y * PrimaryScreen.ScaleY);
            //w = (int)Math.Ceiling(rc.Width * PrimaryScreen.ScaleX);
            //h = (int)Math.Ceiling(rc.Height * PrimaryScreen.ScaleY);
            //Bitmap pagePic = capture.Capture(x, y, w, h);
            //pictureBox1.Image = pagePic;
            //Rectangle rect = ImageRecognition.CalculateCatchArea(pagePic);
            //rect.X += x;
            //rect.Y += y;
            //capture.Draw(rect);
        }
    }
}
