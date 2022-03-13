using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinAchievement.Model
{
    [Serializable]
    public class ExistAchievement
    {
        public int id;
        public string name;
        public string edition;
        public string desc;
        public int reward;
        public string ver;
        public List<string> levels;

        /** 非json字段 **/
        public bool done;
        public OcrAchievement ocrAchievement;
    }
}
