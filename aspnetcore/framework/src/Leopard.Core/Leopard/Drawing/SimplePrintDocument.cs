using Leopard.Helpers.IO;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace Leopard.Drawing
{
    /// <summary>
    /// PrintDocument的简单封装类，更加方便测试
    /// </summary>
    public class SimplePrintDocument<T> : PrintDocument
    {
        private SimplePrintDocument() : base()
        {
            // 没有这个无参构造函数，打开派生类的PrintDocument设计器会报错。并且这个构造函数还不能抛异常            
            // throw new NotImplementedException("请使用带 printerName 参数的构造函数");  
        }

        public SimplePrintDocument(string printerName) : base()
        {
            PrintController = new StandardPrintController();
            //指定打印机
            PrinterSettings.PrinterName = printerName;
            //设置页边距
            PrinterSettings.DefaultPageSettings.Margins.Left = 0;
            PrinterSettings.DefaultPageSettings.Margins.Top = 0;
            PrinterSettings.DefaultPageSettings.Margins.Right = 0;
            PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;
            // 设置尺寸大小。取打印机设置的尺寸
            // 获取到的 PrinterSettings.DefaultPageSettings.PaperSize 不精确，不清楚什么原因
            // 比如设置的是 40mm*20mm
            // 获取到的中百分之一Size为：173,79  =》计算出来的mm为：43mm,20mm
            // 一点点位置，就不管了。   直接获取打印首选项设置的 size 和 横向|纵向
            Size = PrinterSettings.DefaultPageSettings.PaperSize;
            double width_mm = DrawUnitConversion.Inch100ToMM(Size.Width);
            double height_mm = DrawUnitConversion.Inch100ToMM(Size.Height);

            //Size = new PaperSize("custom", (int)DrawUnitConversion.MMToInch100(50), (int)DrawUnitConversion.MMToInch100(20));
            // 50 mm==196 inc
            // 20 mm==78  inc

            // 获取已安装或所选打印机的最大Dpi
            var maxResolution = PrinterSettings.PrinterResolutions.OfType<PrinterResolution>()
                                         .OrderByDescending(r => r.X)
                                         .ThenByDescending(r => r.Y)
                                         .First();
            SetDPI(maxResolution.X, maxResolution.Y);
        }

        /// <summary>
        /// 为每一个派生类都设置一个名字
        /// </summary>
        public string Key { get; protected set; } = "SimplePrint";

        /// <summary>
        /// 自动获取已安装或所选打印机的最大Dpi
        /// </summary>
        public Dpi PrintDpi { get; private set; }
        public int PageCount { get; set; } = 1;
        /// <summary>
        /// 上一次保存图片文件的路径
        /// </summary>
        public string LastTimeSaveImgFilePath { get; private set; }

        /// <summary>
        /// 是否保存为本地文件。默认false
        /// </summary>
        public bool EnableSaveImg { get; private set; } = false;
        /// <summary>
        /// 百分之一英寸尺寸值
        /// </summary>
        protected PaperSize Size { get; set; }

        private string saveDirectory = string.Empty;
        private string saveFileName = string.Empty;
        private bool nameWithRandom = false;
        /// <summary>
        /// 打印纸张对应的像素
        /// </summary>
        private double width_px = 0;
        private double height_px = 0;

        public T Data { get; private set; }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);

            OnBeginPrintValid();
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            // 中间使用像素绘制，像素比较直观
            Bitmap bmp = new Bitmap((int)width_px, (int)height_px);
            Graphics bmpGraphics = Graphics.FromImage(bmp);
            DrawHelper.SetGraphicsHighQuality(bmpGraphics);
            DoDrawing(bmpGraphics, (int)width_px, (int)height_px);

            if (EnableSaveImg)
            {
                LastTimeSaveImgFilePath = BuildSavePath();
                bmp.Save(LastTimeSaveImgFilePath);
                // LOG.INFO($"此次打印的图片保存在：{LastTimeSaveImgFilePath}");
            }

            DrawHelper.SetGraphicsHighQuality(e.Graphics);
            // 最后打印 ，使用 百分之一英寸
            e.Graphics.DrawImage(bmp, 0, 0, Size.Width - 20, Size.Height - 10);

            if (--PageCount > 0)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// 可重写此方法，在打印前对 SetData 数据做验证
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected virtual void OnBeginPrintValid()
        {
            if (Data == null)
                throw new Exception($"请调用{nameof(SetData)}方法设置要打印的数据");

            if (EnableSaveImg)
            {
                if (saveDirectory.IsNullOrEmpty() || saveFileName.IsNullOrEmpty())
                {
                    throw new Exception($"请调用{nameof(SetEnableSaveImg)}方法正确设置图片要保存的路径");
                }
            }
        }

        /// <summary>
        /// 需要绘制什么，就重写这个方法。纸张大小以像素为单位
        /// </summary>
        /// <param name="g"></param>
        /// <param name="width_px">纸张像素宽</param>
        /// <param name="height_px">纸张像素高</param>
        protected virtual void DoDrawing(Graphics g, int width_px, int height_px)
        {
            //边框
            g.DrawRectangle(new Pen(Color.Black, 5), new Rectangle(0, 0, width_px, height_px));
        }

        /// <summary>
        /// 设置打印数据
        /// </summary>
        /// <param name="data"></param>
        public void SetData(T data)
        {
            Data = data;
        }
        /// <summary>
        /// 设置DPI
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetDPI(float x, float y)
        {
            PrintDpi = new Dpi(x, y);

            width_px = Inch100ToPX(Size.Width);
            height_px = Inch100ToPX(Size.Height);
        }

        /// <summary>
        /// 设置保存文件true，保存在默认路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nameWithRandom">文件名自动生成随机数，避免覆盖</param>
        public void SetEnableSaveImg(string fileName, bool nameWithRandom = true)
        {
            SetEnableSaveImg($"{DirectoryHelper.AppBaseDirectory()}\\files\\Label", fileName, nameWithRandom);
        }

        /// <summary>
        /// 设置保存文件true，保存在指定路径
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <param name="nameWithRandom">文件名自动生成随机数，避免覆盖</param>
        public void SetEnableSaveImg(string directory, string fileName, bool nameWithRandom = true)
        {
            saveDirectory = directory;
            saveFileName = fileName;
            this.nameWithRandom = nameWithRandom;
            DirectoryHelper.CreateIfNotExists(saveDirectory);

            EnableSaveImg = true;
        }

        public void SetEnableSaveImgFlase()
        {
            EnableSaveImg = false;
        }

        protected string BuildSavePath()
        {
            string path = string.Empty;
            if (nameWithRandom)
            {
                path = $"{saveDirectory}\\{saveFileName}_{new Random().Next(999)}.jpg";
            }
            else
            {
                path = $"{saveDirectory}\\{saveFileName}.jpg";
            }
            return path;
        }

        /// <summary>
        /// 像素到百分之一英寸
        /// </summary>
        /// <param name="px">像素</param>
        /// <returns></returns>
        protected double PXToInch100(double px)
        {
            return DrawUnitConversion.MMToInch100(DrawUnitConversion.PXToMM(px, PrintDpi, 1));
        }

        /// <summary>
        /// 百分之一英寸转像素
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        protected double Inch100ToPX(double inch)
        {
            return DrawUnitConversion.MMToPX(DrawUnitConversion.Inch100ToMM(inch), PrintDpi, 1);
        }

        /// <summary>
        /// 获取打印的字符大小。默认获取常规大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected float GetFontSize(SimplePrintFontSize size = SimplePrintFontSize.Medium)
        {
            int dpi = (int)PrintDpi.X;

            switch (size)
            {
                case SimplePrintFontSize.Small:
                    return (dpi / 15);
                case SimplePrintFontSize.Large:
                    return (dpi / 8);
                default: // SimplePrintFontSize.Medium
                    return (dpi / 10);
            }
        }
    }

    /// <summary>
    /// 字的大小
    /// </summary>
    public enum SimplePrintFontSize
    {
        Small = 0,
        Medium = 10,
        Large = 20,
    }
}
