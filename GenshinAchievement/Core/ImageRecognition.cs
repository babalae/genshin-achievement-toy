using GenshinAchievement.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinAchievement.Core
{
    public class ImageRecognition
    {

        /// <summary>
        /// 计算出选区矩形
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <returns></returns>
        public static Rectangle CalculateCatchArea(Bitmap imgSrc)
        {
            //Bitmap bitmap = new Bitmap(imgSrc.Width, imgSrc.Height);

            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            List<Rectangle> lines = new List<Rectangle>();
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < data.Height; y++)
                {
                    int s = 0;
                    bool pre = false;
                    Rectangle rect = Rectangle.Empty; // 每一行只有宽度大于 原神界面宽度 1/2 的才保留
                    for (int x = 0; x < data.Width; x++)
                    {
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        if (Math.Abs(r - 235) <= 10 && Math.Abs(g - 225) <= 10 && Math.Abs(b - 225) <= 10)
                        {
                            if (!pre)
                            {
                                if (rect != Rectangle.Empty && rect.Width > data.Width / 2)
                                {
                                    continue; // 直接放弃后续所有
                                }
                                else
                                {
                                    rect = new Rectangle(x, y, 1, 1);
                                }
                            }
                            rect.Width++;
                            pre = true;
                        }
                        else
                        {
                            pre = false;
                        }
                    }
                    // 每一行符合的都存入list
                    if (rect != Rectangle.Empty && rect.Width > data.Width / 2)
                    {
                        lines.Add(rect);
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);

            if (lines.Count >= 2)
            {
                Rectangle rect = new Rectangle(lines[0].X, lines[0].Y,
                    lines[0].Width, lines[lines.Count - 1].Y - lines[0].Y);
                Graphics g = Graphics.FromImage(imgSrc);
                g.DrawRectangle(new Pen(Color.Red), rect);
                return rect;
            }
            return Rectangle.Empty;
        }

        public static Bitmap HighlightBorder(Bitmap imgSrc)
        {
            Bitmap bitmap = new Bitmap(imgSrc.Width, imgSrc.Height);

            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < data.Height; y++)
                {
                    int s = 0;
                    for (int x = 0; x < data.Width; x++)
                    {
                        // write the logic implementation here
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        if (r == 224 && g == 214 && b == 203)
                        {
                            s++;
                        }
                    }
                    if (s * 1.0 / data.Width > 0.8)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            bitmap.SetPixel(x, y, Color.White);
                        }
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);
            return bitmap;
        }

        public static bool Split(Bitmap imgSrc, ref List<OcrAchievement> achievementList)
        {
            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            bool preLineMatched = false; // 连贯的线
            OcrAchievement achievement = null;
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < data.Height; y++)
                {
                    int s = 0;
                    for (int x = 0; x < data.Width; x++)
                    {
                        // write the logic implementation here
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        if (r == 224 && g == 214 && b == 203)
                        {
                            s++;
                        }
                    }
                    if (s * 1.0 / data.Width > 0.8)
                    {
                        if (!preLineMatched)
                        {
                            if (achievement == null)
                            {
                                achievement = new OcrAchievement
                                {
                                    Index = achievementList.Count + 1,
                                    Y1 = y,
                                    ImageSrc = (Bitmap)imgSrc.Clone(),
                                    Width = imgSrc.Width
                                };
                            }
                            else
                            {
                                achievement.Y2 = y;
                                // 高度差太低说明截取错了
                                if (achievement.Y2 - achievement.Y1 < 10)
                                {
                                    imgSrc.UnlockBits(data);
                                    return false;
                                }
                                achievementList.Add(achievement.Clone());
                                achievement = null;
                            }
                        }
                        preLineMatched = true;
                    }
                    else
                    {
                        preLineMatched = false;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);
            return true;
        }

        public static List<Bitmap> Split(Bitmap imgSrc)
        {
            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            List<int> yList = new List<int>();
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < data.Height; y++)
                {
                    int s = 0;
                    for (int x = 0; x < data.Width; x++)
                    {
                        // write the logic implementation here
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        if (r == 224 && g == 214 && b == 203)
                        {
                            s++;
                        }
                    }
                    if (s * 1.0 / data.Width > 0.8)
                    {
                        yList.Add(y);
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);

            List<Bitmap> list = new List<Bitmap>();
            if (yList.Count >= 2)
            {
                for (int i = 1; i < yList.Count; i++)
                {
                    int h = yList[i] - yList[i - 1];
                    if (h > 10)
                    {
                        list.Add(CreateNewBitmap(imgSrc, 0, yList[i - 1], imgSrc.Width, h));
                    }
                }
            }
            return list;
        }

        public static Bitmap CreateNewBitmap(Bitmap imgSrc, int x, int y, int w, int h)
        {
            Bitmap img = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(imgSrc, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
            }
            return img;
        }


        public static bool IsInRow(Bitmap imgSrc)
        {
            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < 1; y++)
                {
                    int s = 0;
                    for (int x = 0; x < data.Width; x++)
                    {
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        // 位于某行内
                        if (Math.Abs(r - 200) <= 20 && Math.Abs(g - 140) <= 15 && Math.Abs(b - 70) <= 10
                            || Math.Abs(r - 109) <= 2 && Math.Abs(g - 100) <= 2 && Math.Abs(b - 93) <= 2)
                        {
                            s++;
                        }
                    }
                    // 像素辨识度很高，所以可以适当调低像素数
                    if (s >= 10)
                    {
                        imgSrc.UnlockBits(data);
                        return true;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);
            return false;
        }
    }
}
