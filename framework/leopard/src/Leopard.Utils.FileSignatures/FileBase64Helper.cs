using FileSignatures;
using Leopard.Crypto;
using Leopard.Utils;
using System;
using System.IO;

namespace Leopard.Helpers.IO
{
    /// <summary>
    /// file base64 帮助类 （文件扩展名识别）
    /// </summary>
    public class FileBase64Helper
    {
        public FileBase64Helper() { }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="base64">base64字符串</param>
        public FileBase64Helper(string base64) : this(base64, Constants.String_Empty, Constants.String_Empty)
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <param name="extension">不带点的文件扩展名（可选：如果实例化时，传入了Extension，则取传入的，没有则先从base64中获取，没有再从stream中提取）</param>
        /// <param name="contentType">浏览器下载文件时需要（可选：如果实例化时，传入了ContentType，则取传入的，没有则先从base64中获取，没有再从stream中提取）</param>
        public FileBase64Helper(string base64, string extension, string contentType)
        {
            Checked.NotNullOrWhiteSpace(base64, nameof(base64));

            if (!CryptoGuide.Base64.IsBase64Value(base64))
            {
                throw new ArgumentException("base64参数传入的非base64字符串", nameof(base64));
            }

            this.Extension = extension;
            this.ContentType = contentType;

            // 先对 Extension，ContentType赋值后，再走base64赋值，做解析
            this.Base64 = base64;
        }

        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="contentType">浏览器下载文件时需要（可选：如果实例化时，传入了ContentType，则取传入的，没有则先从base64中获取，没有再从stream中提取）</param>
        /// <returns>FileBase64Helper</returns>
        public static FileBase64Helper FileToBase64(string filePath, string contentType = Constants.String_Empty)
        {
            Checked.NotNullOrWhiteSpace(filePath, nameof(filePath));

            using (FileStream filestream = new FileStream(filePath, FileMode.Open))
            {
                byte[] bt = new byte[filestream.Length];
                //调用read读取方法
                filestream.Read(bt, 0, bt.Length);
                var base64Str = Convert.ToBase64String(bt);
                string extension = Path.GetExtension(filePath);
                if (!extension.IsNullOrWhiteSpace2())
                {
                    extension = extension.Substring(extension.IndexOf(".") + 1);
                }
               
                return new FileBase64Helper(base64Str, extension, contentType);
            }
        }

        #region 私有参数
        /// <summary>
        /// 文件流
        /// </summary>
        private MemoryStream _stream { get; set; }
        /// <summary>
        /// 检索前100个字符，假如有,则去掉,前面部分
        /// </summary>
        private const int len = 100;

        /// <summary>
        /// base64字符串
        /// </summary>
        protected string _Base64 { get; set; }
        #endregion

        #region 公共属性
        private string _Extension = Constants.String_Empty;
        /// <summary>
        /// 扩展文件名(不带点)
        /// 如果实例化时，传入了Extension，则取传入的，没有则先从base64中获取，没有再从stream中提取
        /// </summary>
        public string Extension {
            get
            {
                if (string.IsNullOrEmpty(_Extension))
                {
                    _Extension = FileFormat.Extension;
                }
                return _Extension;
            }
            set
            {
                _Extension = value;
            }
        }
        /// <summary>
        /// 原始Base64字符串.(传入的原base64)
        /// </summary>
        public string OriginalBase64 { get; private set; }
        /// <summary>
        /// base64字符串 （不包含扩展名信息的base64字符串）
        /// </summary>
        /// <remarks></remarks>
        public string Base64
        {
            get
            {
                return _Base64;
            }
            set
            {
                string file = value;
                OriginalBase64 = file;
                if (string.IsNullOrEmpty(file))
                {
                    _Base64 = null;
                    Extension = null;
                    return;
                }

                int count = file.IndexOf(',', 0, len);

                if (count >= 0)
                {
                    string strExtension = file.Remove(file.IndexOf(';'));

                    if (string.IsNullOrEmpty(Extension))
                    {
                        Extension = "." + strExtension.Substring(strExtension.IndexOf('/') + 1);
                    }

                    if (string.IsNullOrEmpty(ContentType))
                    {
                        int Start = strExtension.IndexOf(':');
                        ContentType = strExtension.Substring(Start + 1);
                    }

                    file = file.Substring(count + 1);
                }

                _Base64 = file;
            }
        }

        private string _ContentType = Constants.String_Empty;
        /// <summary>
        /// 如果要使用HtmlBase64请指定ContentType。
        /// 如果实例化时，传入了ContentType，则取传入的，没有则先从base64中获取，没有再从stream中提取
        /// 部分是支持浏览器直接查看，部分是下载
        /// </summary>
        /// <remarks>例如:application/pdf;image/jpeg;等</remarks>
        public string ContentType
        {
            get
            {
                if (string.IsNullOrEmpty(_ContentType))
                {
                    // FileFormat.MediaType 返回不带点的后缀
                    _ContentType = FileFormat.MediaType;
                }
                return _ContentType;
            }
            set
            {
                _ContentType = value;
            }
        }

        /// <summary>
        /// 支持浏览器打开的base64
        /// </summary>
        /// <remarks></remarks>
        public string HtmlBase64
        {
            get
            {
                if (string.IsNullOrEmpty(ContentType))
                {
                    throw new Exception("未指定ContentType");
                }
                return string.Format("data:{0};base64,{1}", ContentType, Base64);
            }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get { return Stream.Length; } }

        /// <summary>
        /// 文件流
        /// </summary>
        public MemoryStream Stream
        {
            get
            {
                if (_stream == null)
                {
                    string base64 = Base64.Replace(' ', '+');
                    _stream = new MemoryStream(Convert.FromBase64String(base64));
                }
                return _stream;
            }
        }

        private FileFormat fileFormat = null;
        public FileFormat FileFormat
        {
            get
            {
                if (fileFormat == null)
                {
                    var inspector = new FileFormatInspector();
                    fileFormat = inspector.DetermineFileFormat(Stream);
                }

                return fileFormat;
            }
        }

        #endregion
    }
}
