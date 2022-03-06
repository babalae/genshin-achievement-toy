using GenshinAchievement.Utils;
using System;
using System.Drawing;

namespace GenshinAchievement.Core
{
    public class ImageCapture
    {
        IntPtr hwnd;
        IntPtr hdc;

        public void Start()
        {
            hwnd = Native.GetDesktopWindow();
            hdc = Native.GetDC(hwnd);
        }

        //public void Draw(Rectangle rect)
        //{
        //    Graphics g = Graphics.FromHdc(hdc);
        //    g.DrawRectangle(new Pen(Color.Red), rect);
        //}

        public Bitmap Capture(int x, int y, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics bmpGraphic = Graphics.FromImage(bmp);
            //get handle to source graphic
            IntPtr bmpHdc = bmpGraphic.GetHdc();

            //copy it
            bool res = Native.StretchBlt(bmpHdc, 0, 0, w, h,
                hdc, x, y, w, h, Native.CopyPixelOperation.SourceCopy);
            bmpGraphic.ReleaseHdc();

            return bmp;
        }

        public void Stop()
        {
            Native.ReleaseDC(hwnd, hdc);
        }
    }
}
