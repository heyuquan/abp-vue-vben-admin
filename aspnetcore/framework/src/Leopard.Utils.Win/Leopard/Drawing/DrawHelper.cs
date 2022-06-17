using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Leopard.Drawing
{
    // C#用Graphics书写文字时计算字符串所占的像素单位大小  (比如做冒号对齐时需要)
    // https://blog.csdn.net/Libby1984/article/details/77879119
    // System.Drawing.Graphics g = e.Graphics; // 获得一个Graphics实例
    // string str = "string";
    // System.Drawing.Font font = new System.Drawing.Font(new System.Drawing.FontFamily(this.FontFamily.Source), 32);
    // System.Drawing.SizeF size = g.MeasureString(str, font);  // 计算字符串所需要的大小


    public class DrawHelper
    {
        public class DPI
        {
            // C# 获取DPI的几种方式
            // https://blog.csdn.net/htiannuo/article/details/77086550


            /// <summary>
            /// 从传入的图片获取DPI（仅适用于windows）
            /// </summary>
            /// <param name="Img"></param>
            /// <returns></returns>
            public static Dpi GetDpiFromImage(Bitmap Img)
            {
                return new Dpi(Img.HorizontalResolution, Img.VerticalResolution);
            }

            /// <summary>
            /// 从显示器获取DPI（仅适用于windows）
            /// </summary>
            /// <returns></returns>
            public static Dpi GetDpiFromDisplay()
            {
                float x, y;
                //获取显示器的 Dpi
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    x = graphics.DpiX;
                    y = graphics.DpiY;
                }
                return new Dpi(x, y);
            }

            /// <summary>
            /// 设置图像的DPI（仅适用于windows）
            /// DPI在印刷中起到很的作用，决定印刷的质量。
            /// 但是c#中GDI都是采用默认的DPI--即你显示器的DPI。
            /// 这个就比较麻烦了，比如进入打印，或者印刷往往尺寸不对。
            /// </summary>
            /// <param name="Img"></param>
            /// <param name="dpi"></param>
            public static void SetImageDpi(Bitmap Img, Dpi dpi)
            {
                Img.SetResolution(dpi.X, dpi.Y);
            }
        }

        /// <summary>
        /// 给图样Graphics设置为高清高质量模式
        /// </summary>
        /// <param name="g"></param>
        /// <param name="isDocumentPrint">true-是打印机打印图像；false-只是保存图像文件</param>
        public static void SetGraphicsHighQuality(Graphics g, bool isDocumentPrint)
        {
            // 绘图质量
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿   // ps 使用HighQuality在画矩形边框后打印出来，有锯齿
            // 图像合成质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            // drawstring画图，消除锯齿
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (isDocumentPrint)
            {
                // 使用内插模式在缩放期间控制图像质量
                // https://docs.microsoft.com/zh-cn/windows/win32/gdiplus/-gdiplus-using-interpolation-mode-to-control-image-quality-during-scaling-use?redirectedfrom=MSDN
                // g.InterpolationMode = InterpolationMode.HighQualityBicubic;   设置为这个，打印出来的图像会模糊...
                // 原因：http://cn.voidcc.com/question/p-effcogqr-uh.html
                // 当进行插值时，GDI +通常将像素的中心偏移半个像素。这在缩放时可能会产生不良影响，出现将缩放后的图像向上和向左移动的现象。使用PixelOffsetMode.Half将像素移回到它们“所属”的位置。
                // 原因：高质量的双线性和双三次插值模式似乎将边缘像素与图像边界之外的假设透明像素混合在一起，从而在边缘创建半透明边缘。

                // 所以：打印设置为 NearestNeighbor + Half

                // DrawImage 是设备相关的函数，换言之就是，DrawImage会把屏幕的参数带上，所以，它绘制图像的DPI基本都是96。
                // Graphics.DrawImage 打出来的图片变模糊问题 
                // https://www.nuomiphp.com/eplan/399743.html
                // https://blog.csdn.net/pengcwl/article/details/7868344
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                // 对于NearestNeighbor，您需要将PixelOffsetMode设置为Half，否则顶部和左侧会占用图像的一半像素。这实际上在上方的蓝色边框上可见。
                // https://www.nuomiphp.com/eplan/399743.html
                // https://stackoverflow.com/questions/20776605/missing-half-of-first-pixel-column-after-a-graphics-transform-scale
                // https://stackoverflow.com/questions/10222774/bitmap-not-aligning-properly-in-panel
                // 像素的偏移模式
                g.PixelOffsetMode = PixelOffsetMode.Half;
            }
            else
            {
                // 设置 NearestNeighbor 在大图转小图的时候，小图会带锯齿
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
        }
    }
}
