using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Drawing
{
    // px：像素
    // dpi：dots per inch ， 直接来说就是一英寸多少个像素点。我一般称作像素密度，简称密度
    // 分辨率： 横纵2个方向的像素点的数量
    // 屏幕尺寸： 屏幕对角线的长度。

    // 1英寸=25.4mm=96DPI，那么1mm=96/25.4DPI
    // 毫米和英寸的换算：打印机是以英寸为单位的，纸张设置是以毫米为单位的，所以需要转换

    // 像素与毫米的转换
    // 转换还需要知道另一个参数：DPI（每英寸多少点）
    // 象素数 / DPI = 英寸数
    // 英寸数* 25.4 = 毫米数

    /// <summary>
    /// Drawing相关单位转换类
    /// 毫米数 ＝（像素/DPI）* 25.4
    /// 像素=(毫米数/25.4)*DPI
    /// 1英寸=25.4mm=96DPI，那么1mm=96/25.4DPI
    /// </summary>
    public class DrawUnitConversion
    {
        /// <summary>
        /// 1英寸的物理长度：2.54厘米，这里表示25.4毫米
        /// </summary>
        public readonly static double singleLengthD = 25.4;

        /// <summary>
        /// 1英寸的物理长度：2.54厘米，这里表示25.4毫米
        /// </summary>
        public readonly static decimal singleLengthM = 25.4m;

        ///// <summary>
        ///// 毫米转像素。根据显示器的分辨率计算像素（windows）
        ///// </summary>
        ///// <param name="mm">毫米</param>
        ///// <returns></returns>
        //public static double MMToPXByDisplay(double mm)
        //{
        //    // https://blog.csdn.net/weixin_39630855/article/details/111820301

        //    System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
        //    IntPtr hdc = g.GetHdc();
        //    int width = GetDeviceCaps(hdc, 4);      // HORZRES
        //    int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL
        //    g.ReleaseHdc(hdc);
        //    return (((double)pixels / (double)width) * (double)mm);
        //}
        //[DllImport("gdi32.dll")]
        //private static extern int GetDeviceCaps(IntPtr hdc, int Index);

        /// <summary>
        /// 毫米转像素
        /// 像素=(毫米数/25.4)*DPI
        /// </summary>
        /// <param name="mmLength">毫米</param>
        /// <param name="dpi">DPI</param>
        /// <param name="imageMultiple">图片物理倍数</param>
        /// <returns>PX</returns>
        public static double MMToPX(double mmLength, Dpi dpi, int imageMultiple = 1)
        {
            //像素=(毫米数/25.4)*DPI
            if (dpi.Y > dpi.X)
            {
                return ((mmLength / singleLengthD) * dpi.Y) * imageMultiple;
            }
            else
            {
                return ((mmLength / singleLengthD) * dpi.X) * imageMultiple;
            }
        }

        /// <summary>
        /// 像素转毫米
        /// </summary>
        /// <param name="pxLength">像素</param>
        /// <param name="dpi">DPI</param>
        /// <param name="imageMultiple">图片物理倍数</param>
        /// <returns>MM</returns>
        public static double PXToMM(double pxLength, Dpi dpi, int imageMultiple = 1)
        {
            //毫米数 ＝（像素 / DPI）*25.4
            if (dpi.Y > dpi.X)
            {
                return ((pxLength / dpi.Y) * singleLengthD) / imageMultiple;
            }
            else
            {
                return ((pxLength / dpi.X) * singleLengthD) / imageMultiple;
            }
        }


        /// <summary>
        /// 毫米转像素
        /// 像素=(毫米数/25.4)*DPI
        /// </summary>
        /// <param name="mmLength">毫米</param>
        /// <param name="dpi">DPI</param>
        /// <param name="imageMultiple">图片物理倍数</param>
        /// <returns>PX</returns>
        public static decimal MMToPX(decimal mmLength, Dpi dpi, int imageMultiple = 1)
        {
            //像素=(毫米数/25.4)*DPI
            if (dpi.Y > dpi.X)
            {
                return ((mmLength / singleLengthM) * (decimal)dpi.Y) * imageMultiple;
            }
            else
            {
                return ((mmLength / singleLengthM) * (decimal)dpi.X) * imageMultiple;
            }
        }

        /// <summary>
        /// 像素转毫米
        /// </summary>
        /// <param name="pxLength">像素</param>
        /// <param name="dpi">DPI</param>
        /// <param name="imageMultiple">图片物理倍数</param>
        /// <returns>MM</returns>
        public static decimal PXToMM(decimal pxLength, Dpi dpi, int imageMultiple = 1)
        {
            //毫米数 ＝（像素 / DPI）*25.4
            if (dpi.Y > dpi.X)
            {
                return ((pxLength / (decimal)dpi.Y) * singleLengthM) / imageMultiple;
            }
            else
            {
                return ((pxLength / (decimal)dpi.X) * singleLengthM) / imageMultiple;
            }
        }

        /// <summary>
        /// 毫米到百分之一英寸
        /// eg：PrintDocument对象识别到打印机设置默认纸张大小对象 PrinterSettings.DefaultPageSettings.PaperSize 就是将毫米转到百分之一英寸后的值
        /// </summary>
        /// <param name="mm">毫米</param>
        /// <returns></returns>
        public static double MMToInch100(double mm)
        {
            return (mm * 100.0D / 25.4D);
        }

        /// <summary>
        /// 百分之一英寸转毫米
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public static double Inch100ToMM(double inch)
        {
            return ((inch * 25.4) / 100);
        }

        /// <summary>
        /// 像素到百分之一英寸
        /// </summary>
        /// <param name="px">像素</param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static double PXToInch100(double px, Dpi dpi)
        {
            return MMToInch100(PXToMM(px, dpi, 1));
        }

        /// <summary>
        /// 百分之一英寸转像素
        /// </summary>
        /// <param name="inch"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public static double Inch100ToPX(double inch, Dpi dpi)
        {
            return MMToPX(Inch100ToMM(inch), dpi, 1);
        }

    }
}
