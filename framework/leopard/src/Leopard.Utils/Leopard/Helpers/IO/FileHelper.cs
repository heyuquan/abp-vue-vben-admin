using JetBrains.Annotations;
using Leopard.Http;
using Leopard.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Leopard.Helpers.IO
{
    /// <summary>
    /// 文件相关帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Checks and deletes given file if it does exists.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static bool DeleteIfExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            File.Delete(filePath);
            return true;
        }

        /// <summary>
        /// Gets extension of a file.
        /// </summary>
        /// <param name="fileNameWithExtension"></param>
        /// <returns>
        /// Returns extension without dot.
        /// Returns null if given <paramref name="fileNameWithExtension"></paramref> does not include dot.
        /// </returns>
        [CanBeNull]
        public static string GetExtension([NotNull] string fileNameWithExtension)
        {
            Checked.NotNull(fileNameWithExtension, nameof(fileNameWithExtension));

            var lastDotIndex = fileNameWithExtension.LastIndexOf('.');
            if (lastDotIndex < 0)
            {
                return null;
            }

            return fileNameWithExtension.Substring(lastDotIndex + 1);
        }

        /// <summary>
        /// 判断目标是文件夹还是文件。(通过获取路径是否包含扩展后缀)
        /// (PS：在保存文件时，如果没有给文件设置后缀，则路径会被识别为目录，会报：拒绝访问的异常)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true-文件，false-文件夹</returns>
        public static bool CheckIsFile(string path)
        {
            return !DirectoryHelper.CheckIsDir(path);
        }

        /// <summary>
        /// 检查文件路径正确性，抛异常
        /// </summary>
        public static void CheckIsFileWithException(string path)
        {
            if (!CheckIsFile(path))
            {
                throw new ArgumentException("应该输入正确的文件路径", nameof(path));
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
                MD5 md5 = MD5.Create();
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
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="fileName">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<string> ReadAllTextAsync(string fileName)
        {
            using (var reader = File.OpenText(fileName))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="fileName">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            CheckIsFileWithException(fileName);
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
                return result;
            }
        }

        /// <summary>
        /// 读取文件内容为行列表
        /// 读取以\n做分隔。如果最后要StringBuilder组合起来时，要带上\n如：sbuilder.AppendJoin('\n', Lines')
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string[] ReadTextAsLines(string fileName)
        {
            CheckIsFileWithException(fileName);

            StreamReader sr = File.OpenText(fileName);
            string fileContent = sr.ReadToEnd();
            sr.Close();
            return fileContent.Split('\n');
        }


        /// <summary>
        /// Opens a text file, reads content without BOM
        /// </summary>
        /// <param name="fileName">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<string> ReadTextWithoutBomAsync(string fileName)
        {
            CheckIsFileWithException(fileName);
            var content = await ReadAllBytesAsync(fileName);

            return Utf8BomHelper.ReadStringFromByteWithoutBom(content);
        }

        /// <summary>
        /// 保存对象到xml文件
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <param name="source">数据源，会序列化为xml再存储</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void SaveXml<T>(string fileName, T source, bool isAppend = false)
        {
            CheckIsFileWithException(fileName);

            DirectoryHelper.CreateIfNotExists(fileName);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fileName))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(fileName, mode, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, source);
                stream.Flush();
            }
        }

        /// <summary>
        /// 保存text到文件(没有文件会创建新文件)
        /// </summary>
        /// <param name="fileName">文件全路径，如果没带后缀，会自动补上默认.xml</param>
        /// <param name="text">数据源</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void Save(string fileName, string text, bool isAppend = false)
        {
            CheckIsFileWithException(fileName);

            DirectoryHelper.CreateIfNotExists(fileName);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fileName))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(fileName, mode, FileAccess.Write, FileShare.None))
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
        /// <param name="fileName">文件全路径</param>
        /// <param name="data">数据源字节数组</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        public static void Save(string fileName, byte[] data, bool isAppend = false)
        {
            CheckIsFileWithException(fileName);

            DirectoryHelper.CreateIfNotExists(fileName);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fileName))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            using (Stream stream = new FileStream(fileName, mode, FileAccess.Write, FileShare.None))
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
        /// <param name="srcStream"></param>
        /// <param name="fileName">文件全路径</param>
        /// <param name="isAppend">是否追加到已有文本后面；不追加则先清空，再写入文件</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<bool> SaveAsync(Stream srcStream, string fileName, bool isAppend = false)
        {
            if (srcStream == null)
                return false;

            CheckIsFileWithException(fileName);

            const int BuffSize = 32768;
            var result = true;
            Stream dstStream = null;
            var buffer = new byte[BuffSize];

            DirectoryHelper.CreateIfNotExists(fileName);

            FileMode mode = FileMode.OpenOrCreate;
            if (File.Exists(fileName))
            {
                mode = isAppend ? FileMode.Append : FileMode.Truncate;
            }

            try
            {
                await using (dstStream = File.Open(fileName, mode))
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

            return (result && File.Exists(fileName));
        }

        /// <summary>
        /// 读取文件，并对其stream做处理
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <param name="action">读取后，处理数据</param>
        public static void HandleFile(string fileName, Action<Stream> action)
        {
            CheckIsFileWithException(fileName);
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            DirectoryHelper.CreateIfNotExists(fileName);

            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                action(stream);
            }
        }

        #endregion

        #region 查找
        /// <summary>
        /// 从文件中找关键字
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="word"></param>
        public static FindTextResult FindInFile(string fileName, string word)
        {
            CheckIsFileWithException(fileName);

            string[] temp = ReadTextAsLines(fileName);;
            FindTextResult findTextResult = null;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].IndexOf(word) != -1)
                {
                    if (findTextResult is null)
                    {
                        findTextResult = new FindTextResult();
                        findTextResult.FileFullName = fileName;
                    }
                    findTextResult.Matchs.Add(new FindTextItem
                    {
                        LineText = temp[i].Trim(),
                        LineNo = i + 1
                    });
                }
            }
            return findTextResult;
        }
        /// <summary>
        /// 从文件夹中找关键字
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="suffix"></param>
        /// <param name="word"></param>
        public static List<FindTextResult> FindInDirectory(string foldername, string suffix, string word)
        {
            DirectoryHelper.CheckIsDirWithException(foldername);

            List<FindTextResult> result = new List<FindTextResult>();
            DirectoryInfo dif = new DirectoryInfo(foldername);

            foreach (DirectoryInfo di in dif.GetDirectories())
            {
                result.AddRange(FindInDirectory(di.FullName, suffix, word));
            }

            foreach (FileInfo f in dif.GetFiles())
            {
                if (string.IsNullOrEmpty(suffix) || f.Extension == suffix)
                {
                    var temp = FindInFile(f.FullName, word);
                    if (temp is not null)
                    {
                        result.Add(temp);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
