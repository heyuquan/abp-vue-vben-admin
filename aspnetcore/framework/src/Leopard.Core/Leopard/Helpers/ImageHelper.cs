using Leopard.Crypto;
using Leopard.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            FileHelper.CheckFileExistWithException(imageFilePath);
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

        /// <summary>
        /// 改变图片大小
        /// </summary>  
        /// <param name="sFile">原图片绝对路径</param>    
        /// <param name="dFile">图片要保存的绝对路径</param>    
        /// <param name="dHeight">高度（像素）</param>    
        /// <param name="dWidth">宽度（像素）</param>    
        /// <param name="imageQualityValue">图片要保存的压缩质量，该参数的值为1至100的整数，数值越大，保存质量越好</param>
        /// <returns></returns>    
        public static bool Reseize(string sFile, string dFile, int dHeight, int dWidth, int imageQualityValue = 100)
        {
            FileHelper.CheckFileExistWithException(sFile);

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            DrawHelper.SetGraphicsHighQuality(g);

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            try
            {
                return SaveImageForSpecifiedQuality(ob, dFile, tFormat, imageQualityValue);
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>
        /// 按指定的压缩质量及格式保存图片（微软的Image.Save方法保存到图片压缩质量为75)
        /// </summary>
        /// <param name="sourceImage">要保存的图片的Image对象</param>
        /// <param name="savePath">图片要保存的绝对路径</param>
        /// <param name="dFormat">指定图片保存的格式(传null默认取：Jpeg处理方式)</param>
        /// <param name="imageQualityValue">图片要保存的压缩质量，该参数的值为1至100的整数，数值越大，保存质量越好</param>
        /// <returns>保存成功，返回true；反之，返回false</returns>
        public static bool SaveImageForSpecifiedQuality(Image sourceImage, string savePath, ImageFormat dFormat, int imageQualityValue = 100)
        {
            // C# System.Drawing.Imaging下的 Encoder 的一些属性
            // https://blog.csdn.net/linjf520/article/details/7405844
            // 已经可以实现高质量图片保存，但是还没有photoshop清楚，应该是net内置的处理类没有ps那么牛逼

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, imageQualityValue);
            encoderParameters.Param[0] = encoderParameter;
            try
            {
                if (dFormat == null)
                    dFormat = ImageFormat.Jpeg;
                ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == dFormat.Guid);
                FileHelper.EnsureDirExists(savePath);
                if (imageCodecInfo != null)
                {
                    sourceImage.Save(savePath, imageCodecInfo, encoderParameters);
                }
                else
                {
                    if (dFormat == null)
                        sourceImage.Save(savePath);
                    else
                        sourceImage.Save(savePath, dFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩图片
        /// (内部原理：每次将图片质量降低一点，直到图片尺寸小于size)
        /// </summary>
        /// <param name="sFile">原图片地址的绝对路径</param>
        /// <param name="dFile">压缩后保存图片地址的绝对路径</param>
        /// <param name="size">压缩后,图片的最大大小</param>
        /// <returns></returns>
        public static bool Compress(string sFile, string dFile, int size)
        {
            return Inner_Compress(sFile, dFile, size, 90);
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址的绝对路径</param>
        /// <param name="dFile">压缩后保存图片地址的绝对路径</param>
        /// <param name="size">压缩后图片的最大大小(kb)</param>
        /// <param name="imageQualityValue">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        private static bool Inner_Compress(string sFile, string dFile, int size, int imageQualityValue, bool sfsc = true)
        {
            FileHelper.CheckFileExistWithException(sFile);
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile); return true;
            }
            Image iSource = Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth; sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            DrawHelper.SetGraphicsHighQuality(g);

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            try
            {
                bool isSaveOk = SaveImageForSpecifiedQuality(ob, dFile, tFormat, imageQualityValue);
                if (isSaveOk)
                {
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        imageQualityValue = imageQualityValue - 10;
                        return Inner_Compress(sFile, dFile, size, imageQualityValue, false);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }

}
