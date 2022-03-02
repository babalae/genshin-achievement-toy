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
        /// 取指定颜色
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <returns></returns>
        public static Bitmap HighlightPic(Bitmap imgSrc)
        {
            Bitmap bitmap = new Bitmap(imgSrc.Width, imgSrc.Height);

            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        // write the logic implementation here
                        ptr += 3;
                        byte b = ptr[0], g = ptr[1], r = ptr[2];
                        if (Math.Abs(r - 200) <= 20 && Math.Abs(g - 140) <= 15 && Math.Abs(b - 70) <= 10
                            || Math.Abs(r - 109) <= 2 && Math.Abs(g - 100) <= 2 && Math.Abs(b - 93) <= 2)
                        //if (r == 224 && g == 214 && b == 203)
                        {
                            bitmap.SetPixel(j, i, Color.White);
                        }
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            imgSrc.UnlockBits(data);
            return bitmap;
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

        public static bool Split(Bitmap imgSrc, ref List<Achievement> achievementList)
        {
            BitmapData data = imgSrc.LockBits(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            bool preLineMatched = false; // 连贯的线
            Achievement achievement = null;
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
                                achievement = new Achievement
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
