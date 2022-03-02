using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinAchievement.Core
{
    [Serializable]
    class ExistAchievement
    {
        public int id;
        public string name;
        public string desc;
        public int reward;
        public string ver;
    }
}
