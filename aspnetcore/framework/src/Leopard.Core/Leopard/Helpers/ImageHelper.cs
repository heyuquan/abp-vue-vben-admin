using Leopard.Crypto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 从文件中转换图片对象到Base64编码
        /// </summary>
        /// <param name="imageFilePath">图片文件路径</param>
        /// <returns></returns>
        public static string ConvertImageToBase64(string imageFilePath)
        {
            Image image = Image.FromFile(imageFilePath);
            return ConvertImageToBase64(image);
        }

        /// <summary>
        /// 转换图片对象到Base64编码
        /// （只有小图片才允许base64传递到前端，因为base64不能缓存，而图片文件是能被缓存的）
        /// </summary>
        /// <param name="image">Image图片对象</param>
        /// <returns></returns>
        public static string ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                byte[] imageBytes = ms.GetBuffer();
                return CryptoGuide.Base64.Encode(imageBytes);
            }
        }

        /// <summary>
        /// base64字符串转为图片
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }
    }
}
