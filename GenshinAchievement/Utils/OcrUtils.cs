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
            Windows.Globalization.Language lang = new Windows.Globalization.Language("zh-Hans-CN");
            OcrEngine engine = OcrEngine.TryCreateFromLanguage(lang);
            foreach (OcrAchievement a in achievementList)
            {
                string r = await a.Ocr(engine);
                Console.WriteLine(r);
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
