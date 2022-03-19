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

        public ExistAchievement Matching(string edition, OcrAchievement ocrAchievement, bool onlyChinese = false)
        {
            if (string.IsNullOrEmpty(ocrAchievement.OcrText))
            {
                return null;
            }

            double max = 0;
            ExistAchievement maxMatch = null;
            foreach (ExistAchievement existAchievement in All[edition])
            {
                double n = Matching(ocrAchievement, existAchievement, onlyChinese);
                if (n > max)
                {
                    max = n;
                    maxMatch = existAchievement;
                }

            }
            if (max > 0.7 && maxMatch != null && !string.IsNullOrWhiteSpace(ocrAchievement.OcrText))
            {
                if (ocrAchievement.OcrText.Contains("达成"))
                {
                    ocrAchievement.Match = maxMatch;
                    maxMatch.ocrAchievement = ocrAchievement;
                    // 成就集合要再次匹配描述，并把下级成就给完成
                    if (maxMatch.levels != null && maxMatch.levels.Count > 1)
                    {
                        MatchingMutilLevels(ocrAchievement, maxMatch, List2Dic(All[edition]));
                    }
                    else
                    {
                        //if(max < 0.9)
                        //{
                        //    Console.WriteLine($"{ocrAchievement.OcrAchievementName + ocrAchievement.OcrAchievementDesc} 最大匹配 {maxMatch?.name + maxMatch?.desc} 匹配度 {max}");
                        //}
                        maxMatch.done = true;
                    }
                }
                else if (maxMatch.levels != null && maxMatch.levels.Count > 1)
                {
                    MatchingMutilLevels(ocrAchievement, maxMatch, List2Dic(All[edition]), false);
                }
            }
            else
            {
                if (ocrAchievement.OcrText.Contains("达成") && !onlyChinese)
                {
                    Matching(edition, ocrAchievement, true);
                }
                Console.WriteLine($"{ocrAchievement.OcrLeftText} 最小匹配 {maxMatch?.name + maxMatch?.desc} 匹配度 {max}");
            }

            return maxMatch;
        }

        private double Matching(OcrAchievement ocr, ExistAchievement exist, bool onlyChinese)
        {
            if (string.IsNullOrEmpty(ocr.OcrLeftText))
            {
                return -1;
            }
            if (onlyChinese)
            {
                return TextUtils.Similarity(TextUtils.RetainChineseString(ocr.OcrLeftText), TextUtils.RetainChineseString(exist.name + exist.desc));
            }
            else
            {
                return TextUtils.Similarity(ocr.OcrLeftText, exist.name + exist.desc);
            }
        }

        /// <summary>
        /// 天地万象总共就4中多等级的
        /// </summary>
        /// <param name="ocr"></param>
        /// <param name="exist"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private void MatchingMutilLevels(OcrAchievement ocr, ExistAchievement exist, Dictionary<int, ExistAchievement> dic, bool done = true)
        {
            if (exist.id == 80127 || exist.id == 80128 || exist.id == 80129)
            {
                if (exist.id == 80129)
                {
                    dic[80129].done = done;
                    dic[80128].done = true;
                    dic[80127].done = true;
                }
                else if (exist.id == 80128)
                {
                    dic[80128].done = done;
                    dic[80127].done = true;
                }
                else if (exist.id == 80127)
                {
                    dic[80127].done = done;
                }
            }
            else if (exist.id == 81026 || exist.id == 81027 || exist.id == 81028)
            {
                if (!string.IsNullOrEmpty(ocr.OcrLeftText))
                {
                    if (ocr.OcrLeftText.Contains("3"))
                    {
                        dic[81028].done = done;
                        dic[81027].done = true;
                        dic[81026].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("2"))
                    {
                        dic[81027].done = done;
                        dic[81026].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("1"))
                    {
                        dic[81026].done = done;
                    }
                }
            }
            else if (exist.id == 81029 || exist.id == 81030 || exist.id == 81031)
            {
                if (!string.IsNullOrEmpty(ocr.OcrLeftText))
                {
                    if (ocr.OcrLeftText.Contains("3"))
                    {
                        dic[81031].done = done;
                        dic[81030].done = true;
                        dic[81029].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("2"))
                    {
                        dic[81030].done = done;
                        dic[81029].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("1"))
                    {
                        dic[81029].done = done;
                    }
                }
            }
            else if (exist.id == 82041 || exist.id == 82042 || exist.id == 82043)
            {
                if (!string.IsNullOrEmpty(ocr.OcrLeftText))
                {
                    if (ocr.OcrLeftText.Contains("50000"))
                    {
                        dic[82043].done = done;
                        dic[82042].done = true;
                        dic[82041].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("20000"))
                    {
                        dic[82042].done = done;
                        dic[82041].done = true;
                    }
                    else if (ocr.OcrLeftText.Contains("5000"))
                    {
                        dic[82041].done = done;
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
