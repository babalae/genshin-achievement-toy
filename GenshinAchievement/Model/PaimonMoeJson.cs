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

        /// <summary>
        /// 按id索引
        /// </summary>
        public Dictionary<int, ExistAchievement> AchievementDic { get; set; }

        public static PaimonMoeJson Builder()
        {
            PaimonMoeJson paimonMoe = new PaimonMoeJson();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            paimonMoe.All = serializer.Deserialize<Dictionary<string, List<ExistAchievement>>>(Properties.Resources.AchievementJson);
            return paimonMoe;
        }

        public ExistAchievement Matching(string edition, OcrAchievement ocrAchievement)
        {
            Dictionary<int, ExistAchievement> dic = List2Dic(All[edition]);
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
                if (!string.IsNullOrWhiteSpace(ocrAchievement.OcrText) && ocrAchievement.OcrText.Contains("达成"))
                {
                    ocrAchievement.Match = maxMatch;
                    maxMatch.ocrAchievement = ocrAchievement;
                    // 成就集合要再次匹配描述，并把下级成就给完成
                    if (maxMatch.levels != null && maxMatch.levels.Count > 1)
                    {
                        MatchingMutilLevels(ocrAchievement, maxMatch, dic);
                    }
                    else
                    {
                        maxMatch.done = true;
                    }
                }
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

        /// <summary>
        /// 天地万象总共就4中多等级的
        /// </summary>
        /// <param name="ocr"></param>
        /// <param name="exist"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private void MatchingMutilLevels(OcrAchievement ocr, ExistAchievement exist, Dictionary<int, ExistAchievement> dic)
        {
            if (exist.id == 80127 || exist.id == 80128 || exist.id == 80129)
            {
                if (exist.id == 80129)
                {
                    dic[80129].done = true;
                    dic[80128].done = true;
                    dic[80127].done = true;
                }
                else if (exist.id == 80128)
                {
                    dic[80128].done = true;
                    dic[80127].done = true;
                }
                else if (exist.id == 80127)
                {
                    dic[80127].done = true;
                }
            }
            else if (exist.id == 81026 || exist.id == 81027 || exist.id == 81028)
            {
                if (!string.IsNullOrEmpty(ocr.OcrAchievementDesc))
                {
                    if (ocr.OcrAchievementDesc.Contains("3"))
                    {
                        dic[81028].done = true;
                        dic[81027].done = true;
                        dic[81026].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("2"))
                    {
                        dic[81027].done = true;
                        dic[81026].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("1"))
                    {
                        dic[81026].done = true;
                    }
                }
            }
            else if (exist.id == 81029 || exist.id == 81030 || exist.id == 81031)
            {
                if (!string.IsNullOrEmpty(ocr.OcrAchievementDesc))
                {
                    if (ocr.OcrAchievementDesc.Contains("3"))
                    {
                        dic[81031].done = true;
                        dic[81030].done = true;
                        dic[81029].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("2"))
                    {
                        dic[81030].done = true;
                        dic[81029].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("1"))
                    {
                        dic[81029].done = true;
                    }
                }
            }
            else if (exist.id == 82041 || exist.id == 82042 || exist.id == 82043)
            {
                if (!string.IsNullOrEmpty(ocr.OcrAchievementDesc))
                {
                    if (ocr.OcrAchievementDesc.Contains("50000"))
                    {
                        dic[82043].done = true;
                        dic[82042].done = true;
                        dic[82041].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("20000"))
                    {
                        dic[82042].done = true;
                        dic[82041].done = true;
                    }
                    else if (ocr.OcrAchievementDesc.Contains("5000"))
                    {
                        dic[82041].done = true;
                    }
                }
            }
        }

        private Dictionary<int, ExistAchievement> List2Dic(List<ExistAchievement> list)
        {
            Dictionary<int, ExistAchievement> dic = new Dictionary<int, ExistAchievement>();
            foreach (ExistAchievement existAchievement in list)
            {
                dic.Add(existAchievement.id, existAchievement);
            }
            return dic;
        }
    }
}
