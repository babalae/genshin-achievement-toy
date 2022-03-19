using GenshinAchievement.Core;
using GenshinAchievement.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Ocr;

namespace GenshinAchievement.Model
{
    [Serializable]
    public class OcrAchievement
    {
        public int Index { get; set; }
        public Bitmap Image { get; set; }
        public string ImagePath { get; set; }
        public Bitmap ImageSrc { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public int Width { get; set; }
        /// <summary>
        /// 原石图标所在Y坐标
        /// </summary>
        public int PrimogemsY1 { get; set; }
        public int PrimogemsY2 { get; set; }

        public string OcrText { get; set; }

        public OcrResult OcrResult { get; set; }

        /// <summary>
        /// 成就名称：位于图片的左上 !当前识别不准没啥用
        /// </summary>
        public string OcrAchievementName { get; set; }
        /// <summary>
        /// 成就描述：位于图片的左下 !当前识别不准没啥用
        /// </summary>
        public string OcrAchievementDesc { get; set; }
        /// <summary>
        /// 成就结果：位于图片的右中，穿过中轴线
        /// </summary>
        public string OcrAchievementResult { get; set; }
        /// <summary>
        /// 成就完成时间：位于图片的右下，接近底部
        /// </summary>
        public string OcrAchievementFinshDate { get; set; }

        /// <summary>
        /// 位于左侧的文字，用于识别与比较相似度
        /// </summary>
        public string OcrLeftText { get; set; }

        public ExistAchievement Match { get; set; }

        public OcrAchievement Clone()
        {
            return new OcrAchievement
            {
                Index = this.Index,
                ImageSrc = this.ImageSrc,
                Y1 = this.Y1,
                Y2 = this.Y2,
                Width = this.Width,
                PrimogemsY1 = this.PrimogemsY1,
                PrimogemsY2 = this.PrimogemsY2,
            };
        }

        public void Split()
        {
            int x = 0, y = Y1, w = Width, h = Y2 - Y1;
            Image = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(Image))
            {
                //g.Clear(Color.Transparent);
                g.DrawImage(ImageSrc, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
            }
        }

        public async Task<string> Ocr(OcrEngine engine)
        {
            double horizontalY = Image.Height * 1.0 / 2;
            double margin = horizontalY / 4; // 用于边缘容错
            double verticalX = Image.Width * 1.0 / 2;
            OcrResult ocrResult = await OcrUtils.RecognizeAsync(ImagePath, engine);

            // 完整结果
            foreach (var line in ocrResult.Lines)
            {
                OcrText += OcrUtils.LineString(line) + Environment.NewLine;
            }

            // 先找出左侧的两个识别区域
            List<OcrLine> leftLines = ocrResult.Lines
                .Where(line => line.Words[0].BoundingRect.Left < verticalX)
                .OrderBy(line => line.Words[0].BoundingRect.Top).ToList();
            foreach (var line in leftLines)
            {
                OcrLeftText += OcrUtils.LineString(line); // 实际用于文本比较的内容
            }
            if (leftLines.Count == 2)
            {
                OcrAchievementName = OcrUtils.LineString(leftLines[0]); // 左上
                OcrAchievementDesc = OcrUtils.LineString(leftLines[1]); // 左下
            }
            else
            {
                Console.WriteLine("左侧区域未识别到2个结果" + leftLines.Count);
            }

            List<OcrLine> rightLines = ocrResult.Lines.Where(line => line.Words[0].BoundingRect.Left > verticalX).ToList();
            foreach (var line in rightLines)
            {
                Windows.Foundation.Rect firstRect = line.Words[0].BoundingRect;
                string lineStr = OcrUtils.LineString(line);

                if ("达成".Equals(lineStr) || (firstRect.Top < horizontalY && firstRect.Bottom > horizontalY))
                {
                    OcrAchievementResult = lineStr; // 右中 // 98 远海牧人的宝藏 失败
                }
                else if (firstRect.Top > horizontalY)
                {
                    OcrAchievementFinshDate = lineStr.Replace(" ", "").Replace("／", "/"); // 右下 去空格
                }
            }

            // 严格识别不到，就默认第一行
            if (string.IsNullOrEmpty(OcrAchievementName) && ocrResult.Lines.Count > 0)
            {
                OcrAchievementName = OcrUtils.LineString(ocrResult.Lines[0]);
            }
            return OcrText;
        }
    }
}
