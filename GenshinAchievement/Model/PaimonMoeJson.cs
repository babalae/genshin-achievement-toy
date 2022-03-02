using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GenshinAchievement.Model
{
    class PaimonMoeJson
    {
        public Dictionary<string, AchievementsType> All { get; set; }

        //public static PaimonMoeJson Builder()
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    PaimonMoeJson paimonMoe = serializer.Deserialize("", Dictionary)
        //}
    }
}
