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
        public string desc;
        public int reward;
        public string ver;

        public static ExistAchievement Init(Dictionary<string, object> dic)
        {
            ExistAchievement e = new ExistAchievement
            {
                id = (int)dic["id"],
                name = (string)dic["name"],
                desc = (string)dic["desc"],
                reward = (int)dic["reward"],
                ver = (string)dic["ver"]
            };
            return e;
        }
    }
}
