using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Print
{
    public class PrintSizeInfo
    {
        /// <summary>
        /// 页面宽度
        /// </summary>
        public int PageWidth { get; set; }
        /// <summary>
        /// 页面高度
        /// </summary>
        public int PageHeight { get; set; }

        /// <summary>
        /// 打印起点位置的X坐标
        /// </summary>
        public int StartX { get; set; }
        /// <summary>
        /// 打印起点位置的Y坐标
        /// </summary>
        public int StartY { get; set; }

        /// <summary>
        /// 打印宽度
        /// </summary>
        public int PrintWidth { get; set; }
        /// <summary>
        /// 打印高度
        /// </summary>
        public int PrintHeight { get; set; }

        /// <summary>
        /// 创建打印尺寸信息
        /// </summary>
        /// <param name="pageWidth"></param>
        /// <param name="pageHeight"></param>
        /// <param name="spaceScale">print和page中间的留白.（eg：传入0.1，则四周留白10%）</param>
        /// <returns></returns>
        public static PrintSizeInfo Create(int pageWidth, int pageHeight, double spaceScale)
        {
            PrintSizeInfo info = new PrintSizeInfo();
            info.PageWidth = pageWidth;
            info.PageHeight = pageHeight;

            // 以短边留白为最大值，  避免边越长，留白越多
            if (pageWidth >= pageHeight)
            {
                info.StartY = (int)((pageHeight * spaceScale) / 2);
                info.StartX = info.StartY;
            }
            else
            {
                info.StartX = (int)((pageWidth * spaceScale) / 2);
                info.StartY = info.StartX;
            }

            info.PrintWidth = pageWidth - info.StartX * 2;
            info.PrintHeight = pageHeight - info.StartY * 2;

            return info;
        }
    }
}
