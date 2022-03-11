using GenshinAchievement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinAchievement.Utils
{
    public class TextUtils
    {

        public static double Similarity(string txt1, string txt2)
        {
            //txt1 = RetainChineseString(txt1);
            //txt2 = RetainChineseString(txt2);
            List<char> sl1 = txt1.ToCharArray().ToList();
            List<char> sl2 = txt2.ToCharArray().ToList();
            //去重
            List<char> sl = sl1.Union(sl2).ToList<char>();

            //获取重复次数
            List<int> arrA = new List<int>();
            List<int> arrB = new List<int>();
            foreach (var str in sl)
            {
                arrA.Add(sl1.Where(x => x == str).Count());
                arrB.Add(sl2.Where(x => x == str).Count());
            }
            //计算商
            double num = 0;
            //被除数
            double numA = 0;
            double numB = 0;
            for (int i = 0; i < sl.Count; i++)
            {
                num += arrA[i] * arrB[i];
                numA += Math.Pow(arrA[i], 2);
                numB += Math.Pow(arrB[i], 2);
            }
            double cos = num / (Math.Sqrt(numA) * Math.Sqrt(numB));
            return cos;
        }

        public static string RetainChineseString(string str)
        {
            StringBuilder chineseString = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 0x4E00 && str[i] <= 0x9FA5) //汉字
                {
                    chineseString.Append(str[i]);
                }
            }
            return chineseString.Length > 0 ? chineseString.ToString() : string.Empty;
        }

        public static string GeneratePaimonMoeJS(string edition, PaimonMoeJson paimonMoeJson)
        {
            string paimonMoeJsItem = "";
            foreach (ExistAchievement existAchievement in paimonMoeJson.All[edition])
            {
                if (existAchievement.done)
                {
                    paimonMoeJsItem += $"[0,{existAchievement.id}],";
                }
            }
            if (paimonMoeJsItem.EndsWith(","))
            {
                paimonMoeJsItem.Substring(paimonMoeJsItem.Length - 2, 1);
            }
            string paimonMoeJs = @"/*
* 复制此处所有内容，
* 在 https://paimon.moe/ 页面按F12打开开发者工具，
* 选择控制台(Console)
* 粘贴并回车执行完成导入
*/
";

                paimonMoeJs+="const b = [" + paimonMoeJsItem + @"];
const a = (await localforage.getItem('achievement')) || { };
            b.forEach(c => { a[c[0]] = a[c[0]] ||{ }; a[c[0]][c[1]] = true})
await localforage.setItem('achievement', a);
            location.href = '/achievement'";
            return paimonMoeJs;
        }
    }
}
