using GenshinAchievement.Core;
using GenshinAchievement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GenshinAchievement.Utils
{
    class OcrUtils
    {

        public static async void Ocr(List<OcrAchievement> achievementList)
        {
            PaimonMoeJson paimonMoeJson = PaimonMoeJson.Builder();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Windows.Globalization.Language lang = new Windows.Globalization.Language("zh-Hans-CN");
            OcrEngine engine = OcrEngine.TryCreateFromLanguage(lang);
            foreach (OcrAchievement a in achievementList)
            {
                string r = await a.Ocr(engine);
                Console.WriteLine(r);
            }
            Console.WriteLine("识别结束");
            string context = "";
            foreach (OcrAchievement a in achievementList)
            {
                paimonMoeJson.Matching("0", a);
                a.Image = null;
                //context += $"------------------\n";
                //context += $"{a.ImagePath}\n";
                context += $"------------------\n";
                context += $"{serializer.Serialize(a)}\n";
                context += $"------------------\n";
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ocr2.txt"), false))
            {
                sw.WriteLine(context);
            }
        }

        public static async Task<OcrResult> RecognizeAsync(string path, OcrEngine engine)
        {
            StorageFile storageFile;
            storageFile = await StorageFile.GetFileFromPathAsync(path);
            IRandomAccessStream randomAccessStream = await storageFile.OpenReadAsync();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
            randomAccessStream.Dispose();
            softwareBitmap.Dispose();
            return ocrResult;
        }

        public static string LineString(OcrLine line)
        {
            string lineStr = "";
            foreach (var word in line.Words)
            {
                lineStr += word.Text;
            }
            return lineStr;
        }
    }
}
