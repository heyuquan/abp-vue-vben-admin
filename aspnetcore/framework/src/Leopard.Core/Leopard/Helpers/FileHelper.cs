using Leopard.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Volo.Abp;

namespace Leopard.Helpers
{
    /// <summary>
    /// 文件相关帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 确保目录存在，不存在，则创建
        /// （若传递的是文件全路径，会截取其对应的目录）
        /// </summary>
        /// <returns></returns>
        public static void EnsureDirExists(string path)
        {
            string dir = IsDir(path) ? path : Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 判断目标是文件夹还是文件
        /// 前提：保证路径不带符号 "."
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true-文件夹，false-文件</returns>
        public static bool IsDir(string path)
        {
            return string.IsNullOrEmpty(Path.GetExtension(path));
        }

        /// <summary>
        /// 判断目标是文件夹还是文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true-文件，false-文件夹</returns>
        public static bool IsFile(string path)
        {
            return !IsDir(path);
        }

        /// <summary>
        /// 将传入的路径，转为当前运行系统的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRuntimeDirectory(string path)
        {
            //ForLinux
            if (IsLinuxRunTime())
                return GetLinuxDirectory(path);
            //ForWindows
            if (IsWindowRunTime())
                return GetWindowDirectory(path);
            return path;
        }

        /// <summary>
        /// 是否 windows 运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowRunTime()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// 是否 linux 运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsLinuxRunTime()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
        /// <summary>
        /// 将传入的路径，转为 linux 运行系统的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLinuxDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace("\\", "/");
        }

        /// <summary>
        /// 将传入的路径，转为 windows 运行系统的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetWindowDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace("/", "\\");
        }

        /// <summary>
        /// 获取应用程序当前目录，如果参数为空，返回目录名.目录名最后是带下划线的.
        /// </summary>
        /// <param name="name">xxx.xx</param>
        /// <returns></returns>
        public static string Local(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                return $"{AppDomain.CurrentDomain.BaseDirectory}{name}";
            }
        }

        #region 下载

        private const string CTYPE_OCTET = "application/octet-stream";
        private static Regex regex = new Regex("attachment;filename=\"([\\w\\-]+)\"", RegexOptions.Compiled);

        /// <summary>
        /// 通过HTTP GET方式下载文件到指定的目录。
        /// </summary>
        /// <param name="url">需要下载的URL</param>
        /// <param name="destDir">需要下载到的目录</param>
        /// <returns>下载后的文件</returns>
        public static string Download(string url, string destDir)
        {
            string file = null;

            try
            {
                WebUtils wu = new WebUtils();
                HttpWebRequest req = wu.GetWebRequest(url, "GET", null);
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                if (CTYPE_OCTET.Equals(rsp.ContentType))
                {
                    file = Path.Combine(destDir, GetFileName(rsp.Headers["Content-Disposition"]));
                    using (System.IO.Stream rspStream = rsp.GetResponseStream())
                    {
                        int len = 0;
                        byte[] buf = new byte[8192];
                        using (FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate))
                        {
                            while ((len = rspStream.Read(buf, 0, buf.Length)) > 0)
                            {
                                fileStream.Write(buf, 0, len);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(wu.GetResponseAsString(rsp, Encoding.UTF8));
                }
            }
            catch (WebException e)
            {
                throw e;
            }
            return file;
        }

        private static string GetFileName(string contentDisposition)
        {
            Match match = regex.Match(contentDisposition);
            if (match.Success)
            {
                return match.Groups[1].ToString();
            }
            else
            {
                throw new Exception("Invalid response header format!");
            }
        }

        #endregion

        /// <summary>
        /// 检查指定文件的md5sum和指定的检验码是否一致。
        /// </summary>
        /// <param name="fileName">需要检验的文件</param>
        /// <param name="checkCode">已知的md5sum检验码</param>
        /// <returns>true/false</returns>
        public static bool CheckMd5sum(string fileName, string checkCode)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString().Equals(checkCode);
            }
        }

        #region xml文件读写

        /// <summary>
        /// 保存对象到xml文件
        /// </summary>
        /// <param name="fullPath">文件全路径，如果没带后缀，会自动补上默认.xml</param>
        /// <param name="source">数据源</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveXmlFile<T>(string fullPath, T source, bool isAppend = false)
        {
            EnsureDirExists(fullPath);
            string extension = Path.GetExtension(fullPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fullPath = $"{fullPath}.xml";
            }

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fullPath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(fullPath, mode, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, source);
                stream.Flush();
            }
        }

        /// <summary>
        /// 保存text到文件
        /// </summary>
        /// <param name="fullPath">文件全路径，如果没带后缀，会自动补上默认.xml</param>
        /// <param name="text">数据源</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveFile(string fullPath, string text, bool isAppend = false)
        {
            EnsureDirExists(fullPath);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fullPath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(fullPath, mode, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(text);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// 保存对象到xml文件
        /// </summary>
        /// <param name="fullPath">文件全路径，如果没带后缀，会自动补上默认.xml</param>
        /// <param name="handle">读取后，处理数据</param>
        public static void HandleXmlFile(string fullPath, Action<Stream> handle)
        {
            Check.NotNullOrWhiteSpace(fullPath, nameof(fullPath));
            Check.NotNull(handle, nameof(handle));

            EnsureDirExists(fullPath);
            string extension = Path.GetExtension(fullPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fullPath = $"{fullPath}.xml";
            }
            using (Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                handle(stream);
            }
        }

        #endregion
    }
}
