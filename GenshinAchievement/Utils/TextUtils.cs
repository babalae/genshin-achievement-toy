﻿using GenshinAchievement.Model;
using GenshinAchievement.Model.UIAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
                    paimonMoeJsItem += $"[{paimonMoeJson.EditionIndexDic[edition]},{existAchievement.id}],";
                }
            }
            if (paimonMoeJsItem.EndsWith(","))
            {
                paimonMoeJsItem.Substring(paimonMoeJsItem.Length - 2, 1);
            }
            // const a = (await localforage.getItem('achievement')) || { };
            string paimonMoeJs = @"/*
* 复制此处所有内容，
* 在 https://paimon.moe/ 页面按F12打开开发者工具，
* 选择控制台(Console)
* 粘贴并回车执行完成导入
*/
";

            paimonMoeJs += "const b = [" + paimonMoeJsItem + @"];
const a = (await localforage.getItem('achievement')) || { };
            b.forEach(c => { a[c[0]] = a[c[0]] ||{ }; a[c[0]][c[1]] = true})
await localforage.setItem('achievement', a);
            location.href = '/achievement'";
            return paimonMoeJs;
        }


        public static string GenerateSeelieMeJS(string edition, PaimonMoeJson paimonMoeJson)
        {
            string jsItem = "";
            foreach (ExistAchievement existAchievement in paimonMoeJson.All[edition])
            {
                if (existAchievement.done)
                {
                    jsItem += $"[{existAchievement.id},\"手动勾选 {existAchievement.ocrAchievement?.OcrAchievementFinshDate}\"],";
                }
            }
            if (jsItem.EndsWith(","))
            {
                jsItem.Substring(jsItem.Length - 2, 1);
            }
            string js = @"/*
* 复制此处所有内容，
* 在 https://seelie.me/ 页面按F12打开开发者工具，
* 选择控制台(Console)
* 粘贴并回车执行完成导入
*/
";
            js += "const z = [" + jsItem + @"];
const a = localStorage.account || 'main'
const b = JSON.parse(localStorage.getItem(`${a}-achievements`)||'{}')
z.forEach(c=>{b[c[0]]={done:true,notes:c[1]}})
localStorage.setItem(`${a}-achievements`,JSON.stringify(b))
localStorage.last_update = (new Date()).toISOString()
location.href='/achievements'";
            return js;
        }

        /// <summary>
        /// 没兼容cocogoat.work多账号模式，导致cocogoat.work出现问题，故弃用
        /// </summary>
        /// <param name="edition"></param>
        /// <param name="paimonMoeJson"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GenerateCocogoatWorkJS(string edition, PaimonMoeJson paimonMoeJson)
        {
            string jsItem = "";
            foreach (ExistAchievement existAchievement in paimonMoeJson.All[edition])
            {
                if (existAchievement.done)
                {
                    jsItem += $"[{existAchievement.id},\"{existAchievement.ocrAchievement?.OcrAchievementFinshDate}\"],";
                }
            }
            if (jsItem.EndsWith(","))
            {
                jsItem.Substring(jsItem.Length - 2, 1);
            }
            string js = @"/*
* 复制此处所有内容，
* 在 https://cocogoat.work/ 页面按F12打开开发者工具，
* 选择控制台(Console)
* 粘贴并回车执行完成导入
*/
";
            js += "const z = [" + jsItem + @"];
const a = '成就导出'
const b = JSON.parse(localStorage.getItem(`cocogoat.v1.${a}`)||'{}')
b.achievements=[]
z.forEach(c=>{b.achievements.push({id:c[0],date:c[1],status:'手动勾选',categoryId:0})})
localStorage.setItem(`cocogoat.v1.${a}`,JSON.stringify(b))
localStorage.setItem(`cocogoat.v1.currentUser`,JSON.stringify(a))
location.href='/achievement'";

            return js;
        }

        public static string GenerateCocogoatWorkJson(string edition, PaimonMoeJson paimonMoeJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<CocogoatAchievement> list = new List<CocogoatAchievement>();
            foreach (ExistAchievement existAchievement in paimonMoeJson.All[edition])
            {
                if (existAchievement.done)
                {
                    CocogoatAchievement cocogoatAchievement = new CocogoatAchievement
                    {
                        id = existAchievement.id,
                        status = "手动勾选",
                        categoryId = paimonMoeJson.EditionIndexDic[edition],
                        date = existAchievement.ocrAchievement == null ? "" : existAchievement.ocrAchievement.OcrAchievementFinshDate
                    };
                    list.Add(cocogoatAchievement);
                }
            }
            return "{\"value\":{\"achievements\":" + serializer.Serialize(list) + "}}";
        }


        public static string GenerateUIAFJson(string edition, PaimonMoeJson paimonMoeJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            UIAFInfo info = new UIAFInfo
            {
                export_app = "genshin-achievement-toy",
                export_app_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                export_timestamp = CurrentTimeStamp(),
                uiaf_version = "v1.0",
            };
            List<UIAFAchievement> list = new List<UIAFAchievement>();
            foreach (ExistAchievement existAchievement in paimonMoeJson.All[edition])
            {
                if (existAchievement.done)
                {
                    UIAFAchievement achievement = new UIAFAchievement
                    {
                        id = existAchievement.id,
                        timestamp = ToTimeStamp(existAchievement.ocrAchievement),
                        current = 30,
                    };
                    list.Add(achievement);
                }
            }
            UIAFData data = new UIAFData
            {
                info = info,
                list = list
            };
            return serializer.Serialize(data);
        }


        /// <summary>
        /// 获取时间戳 可选秒级/毫秒级
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeStamp(bool isMinseconds = false)
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(isMinseconds ? ts.TotalMilliseconds : ts.TotalSeconds);
        }

        public static long ToTimeStamp(OcrAchievement ocrAchievement)
        {
            string ocrDate = ocrAchievement == null ? null : ocrAchievement.OcrAchievementFinshDate;
            if (ocrDate == null || ocrDate.Length != 10 || !ocrDate.Contains("/"))
            {
                return 253402271999;
            }
            try
            {
                DateTime dt = DateTime.ParseExact(ocrDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                var ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 253402271999;
            }
        }
    }
}
