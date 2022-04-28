using Leopard.Http;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Leopard.Helpers
{
    /// <summary>
    /// 文件相关帮助类
    /// </summary>
    public static partial class FileHelper
    {
        /// <summary>
        /// 判断目标是文件夹还是文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true-文件，false-文件夹</returns>
        public static bool IsFile(string path)
        {
            return !IsDir(path);
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
        /// 检查文件是否存在，不存在则抛出Exception异常
        /// </summary>
        /// <param name="filePath"></param>
        public static void CheckFileExistWithException(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件不存在", filePath);
            }
        }

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

        #region 文件读写

        /// <summary>
        /// 保存对象到xml文件
        /// </summary>
        /// <param name="destFilePath">文件全路径</param>
        /// <param name="source">数据源，会序列化为xml再存储</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveXmlFile<T>(string destFilePath, T source, bool isAppend = false)
        {
            EnsureDirExists(destFilePath);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(destFilePath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(destFilePath, mode, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, source);
                stream.Flush();
            }
        }

        /// <summary>
        /// 保存text到文件(没有文件会创建新文件)
        /// </summary>
        /// <param name="destFilePath">文件全路径，如果没带后缀，会自动补上默认.xml</param>
        /// <param name="text">数据源</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveFile(string destFilePath, string text, bool isAppend = false)
        {
            EnsureDirExists(destFilePath);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(destFilePath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(destFilePath, mode, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(text);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// 保存data到文件
        /// </summary>
        /// <param name="destFilePath">文件全路径</param>
        /// <param name="data">数据源字节数组</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveFile(string destFilePath, byte[] data, bool isAppend = false)
        {
            EnsureDirExists(destFilePath);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(destFilePath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(destFilePath, mode, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter sw = new BinaryWriter(stream))
                {
                    sw.Write(data);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// 保存text到文件(没有文件会创建新文件)
        /// </summary>
        public static async Task<bool> SaveFileAsync(Stream srcStream, string destFilePath, bool isAppend = false)
        {
            if (srcStream == null)
                return false;

            const int BuffSize = 32768;
            var result = true;
            Stream dstStream = null;
            var buffer = new byte[BuffSize];

            EnsureDirExists(destFilePath);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(destFilePath))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            try
            {
                await using (dstStream = File.Open(destFilePath, mode))
                {
                    int len;
                    while ((len = await srcStream.ReadAsync(buffer.AsMemory(0, BuffSize))) > 0)
                    {
                        await dstStream.WriteAsync(buffer.AsMemory(0, len));
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (dstStream != null)
                {
                    dstStream.Close();
                    await dstStream.DisposeAsync();
                }
            }

            return (result && File.Exists(destFilePath));
        }

        /// <summary>
        /// 读取文件，并对其stream做处理
        /// </summary>
        /// <param name="destFilePath">文件全路径</param>
        /// <param name="action">读取后，处理数据</param>
        public static void HandleFile(string destFilePath, Action<Stream> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            EnsureDirExists(destFilePath);

            using (Stream stream = new FileStream(destFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                action(stream);
            }
        }

        #endregion
    }
}
