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
    public class PaimonMoeJson
    {
        public Dictionary<string, List<ExistAchievement>> All { get; set; }

        public static PaimonMoeJson Builder()
        {
            PaimonMoeJson paimonMoe = new PaimonMoeJson();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            paimonMoe.All = serializer.Deserialize<Dictionary<string, List<ExistAchievement>>>(Properties.Resources.AchievementJson);
            return paimonMoe;
        }

        public ExistAchievement Matching(string edition, OcrAchievement ocrAchievement)
        {
            double max = 0;
            ExistAchievement maxMatch = null;
            foreach (ExistAchievement existAchievement in All[edition])
            {
                double n = Matching(ocrAchievement, existAchievement);
                if (n > max)
                {
                    max = n;
                    maxMatch = existAchievement;
                }

            }
            if (max > 0.6 && maxMatch != null)
            {
                ocrAchievement.Match = maxMatch;
                maxMatch.done = true;
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
