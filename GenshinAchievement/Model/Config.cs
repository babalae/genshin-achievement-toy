using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinAchievement.Model
{
    public class Config
    {
        public string hotkey_stop = "F11";
        public int capture_interval = 20;

        public string GetHotkeyStop()
        {
            return string.IsNullOrWhiteSpace(hotkey_stop) ? "F11" : hotkey_stop;
        }

        public int GetCaptureInterval()
        {
            return capture_interval == 0 ? 20 : capture_interval;
        }
    }
}
