using GenshinAchievement.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinAchievement.Core
{
    public class YuanShenWindow
    {
        public static uint WM_MOUSEWHEEL = 0x020A; // 滚轮滑动
        public static uint WM_MOUSEMOVE = 0x200; // 鼠标移动
        public static uint WM_LBUTTONDOWN = 0x201; //按下鼠标左键

        public static uint WM_LBUTTONUP = 0x202; //释放鼠标左键

        [Flags]
        public enum WinMsgMouseKey : int
        {
            MK_NONE = 0x0000,
            MK_LBUTTON = 0x0001,
            MK_RBUTTON = 0x0002,
            MK_SHIFT = 0x0004,
            MK_CONTROL = 0x0008,
            MK_MBUTTON = 0x0010,
            MK_XBUTTON1 = 0x0020,
            MK_XBUTTON2 = 0x0040
        }


        public IntPtr HWND { get; set; }
        public YuanShenWindow()
        {

        }

        internal static IntPtr MAKEWPARAM(int direction, float multiplier, WinMsgMouseKey button)
        {
            int delta = (int)(SystemInformation.MouseWheelScrollDelta * multiplier);
            return (IntPtr)(((delta << 16) * Math.Sign(direction) | (ushort)button));
        }

        internal static IntPtr MAKELPARAM(int low, int high)
        {
            return (IntPtr)((high << 16) | (low & 0xFFFF));
        }

        public bool FindYSHandle()
        {
            var pros = Process.GetProcessesByName("YuanShen");
            if (pros.Any())
            {
                HWND = pros[0].MainWindowHandle;
                return true;
            }
            else
            {
                pros = Process.GetProcessesByName("GenshinImpact");
                if (pros.Any())
                {
                    HWND = pros[0].MainWindowHandle;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Focus()
        {
            Native.SendMessage(HWND, 0x0112, (IntPtr)0xF120, (IntPtr)0);
            Native.SetForegroundWindow(HWND);
        }

        public Rectangle GetSize()
        {
            Native.RECT rc = new Native.RECT();
            Native.GetWindowRect(HWND, ref rc);
            return new Rectangle(rc.Left, rc.Top, rc.Right - rc.Left, rc.Bottom - rc.Top);
        }

        public void MouseWheelUp()
        {
            Native.mouse_event(Native.MouseEventFlag.Wheel, 0, 0, 120, 0);
        }

        public void MouseWheelDown()
        {
            Native.mouse_event(Native.MouseEventFlag.Wheel, 0, 0, -120, 0);
        }

        public void MouseMove(int x, int y)
        {
            Native.mouse_event(Native.MouseEventFlag.Absolute | Native.MouseEventFlag.Move, 
                x * 65536 / PrimaryScreen.DESKTOP.Width, y * 65536 / PrimaryScreen.DESKTOP.Height,
                0, 0);
        }

        public void MoveCursor(int x, int y)
        {
            Native.SetCursorPos(x, y);
        }

        public void MouseLeftDown()
        {
            //IntPtr p = (IntPtr)((y << 16) | x);
            //Native.PostMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, p);
            //Native.mouse_event(Native.MouseEventFlag.LeftDown, 0, 0, -120, 0);
            Native.mouse_event(Native.MouseEventFlag.LeftDown, 0, 0, 0, 0);
        }

        public void MouseLeftUp()
        {
            Native.mouse_event(Native.MouseEventFlag.LeftUp, 0, 0, 0, 0);
        }

        public void MouseClick(int x, int y)
        {
            IntPtr p = (IntPtr)((y << 16) | x);
            Native.PostMessage(HWND, WM_LBUTTONDOWN, IntPtr.Zero, p);
            Thread.Sleep(100);
            Native.PostMessage(HWND, WM_LBUTTONUP, IntPtr.Zero, p);
        }
    }
}
