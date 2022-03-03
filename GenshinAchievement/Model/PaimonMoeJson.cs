using GenshinAchievement.Core;
using GenshinAchievement.Utils;
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

        public static PaimonMoeJson Builder()
        {
            PaimonMoeJson paimonMoe = new PaimonMoeJson();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            paimonMoe.All = serializer.Deserialize<Dictionary<string, AchievementsType>>(Properties.Resources.PaimonMoeAchievementJson);
            return paimonMoe;
        }

        public ExistAchievement Matching(string type, OcrAchievement ocrAchievement)
        {
            double max = 0;
            ExistAchievement maxMatch = null;
            foreach (object o in All[type].achievements)
            {
                if (o is Dictionary<string, object>)
                {
                    ExistAchievement existAchievement = ExistAchievement.Init(o as Dictionary<string, object>);
                    double n = Matching(ocrAchievement, existAchievement);
                    if (n > max)
                    {
                        max = n;
                        maxMatch = existAchievement;
                    }
                }
                else if (o is Array)
                {
                    foreach (object a in o as Array)
                    {
                        ExistAchievement existAchievement = ExistAchievement.Init(a as Dictionary<string, object>);
                        double n = Matching(ocrAchievement, existAchievement);
                        if (n > max)
                        {
                            max = n;
                            maxMatch = existAchievement;
                        }
                    }
                }
            }
            if (max > 0.6 && maxMatch != null)
            {
                ocrAchievement.Match = maxMatch;
            }
            else
            {
                Console.WriteLine($"{ocrAchievement.OcrAchievementName} 最小匹配 {maxMatch?.name} 匹配度 {max}");
            }

            return maxMatch;
        }

        private double Matching(OcrAchievement ocr, ExistAchievement exist)
        {
            if (string.IsNullOrEmpty(ocr.OcrAchievementName))
            {
                return -1;
            }
            return TextUtils.Similarity(ocr.OcrAchievementName, exist.name);
        }
    }
}
