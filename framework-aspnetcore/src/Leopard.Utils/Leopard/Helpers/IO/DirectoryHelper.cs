using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Leopard.Helpers.IO
{
    /// <summary>
    /// 本地目录相关
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 获取应用程序当前目录，如果参数为空，返回目录名.目录名最后是带下划线的.
        /// </summary>
        /// <param name="name">xxx.xx</param>
        /// <returns></returns>
        public static string AppBaseDirectory(string name = null)
        {
            // .NET 程序读取当前目录避坑指南
            // https://www.cnblogs.com/kklldog/p/how-to-read-current-dir.html
            // 不使用  Directory.GetCurrentDirectory
            // 不使用  AppContext.BaseDirectory
            // 使用    AppDomain.CurrentDomain.BaseDirectory
            // 使用    Assembly.GetExecutingAssembly().Location

            if (string.IsNullOrEmpty(name))
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                return $"{AppDomain.CurrentDomain.BaseDirectory}{name}";
            }
        }

        #region 特定系统的目录

        /// <summary>
        /// 将传入的路径，转为当前运行系统的路径
        /// 建议：如果把路径存入数据库，那么统一采用 / 作为分隔符，避免系统运行不同平台，路径会乱掉。
        /// 取出后再转为特定平台的路径。  可以调用 GetRuntimeDirectory 方法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRuntimeDirectory(string path)
        {
            //ForLinux
            if (SystemHelper.IsLinuxRunTime())
                return GetLinuxDirectory(path);
            //ForWindows
            if (SystemHelper.IsWindowRunTime())
                return GetWindowDirectory(path);
            return path;
        }

        /// <summary>
        /// 将传入的路径，转为 linux 运行系统的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLinuxDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace(@"\", @"/");
        }

        /// <summary>
        /// 将传入的路径，转为 windows 运行系统的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetWindowDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace(@"/", @"\");
        }

        /// <summary>
        /// 传入目录节点，拼接为当前系统的目录
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            // Path.Combine 可以自动生成符合各个平台运行要求的路径
            // eg: Path.Combine(webHostEnvironment.ContentRootPath, "files", DateTime.UtcNow.ToString("yyyy"),DateTime.UtcNow.ToString("MM"),DateTime.UtcNow.ToString("dd"),"xxx.jpg");
            // windows: d:\appdata\files\2022\11\24\xxx.jpg
            // linux:   /var/appdata/files/2022/11/24/xxx.jpg

            return Path.Combine(paths);
        }

        #endregion

        /// <summary>
        /// 确保目录存在，不存在，则创建
        /// （若传递的是文件全路径，会截取其对应的目录）
        /// </summary>
        /// <returns></returns>
        public static void CreateIfNotExists(string path)
        {
            string dir = CheckIsDir(path) ? path : Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directory"></param>
        public static void DeleteIfExists(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }
        }

        /// <summary>
        /// 删除目录（循环删除子目录）
        /// </summary>
        public static void DeleteIfExists(string directory, bool recursive)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive);
            }
        }

        /// <summary>
        /// 判断目标是文件夹还是文件.(通过获取路径是否包含扩展后缀)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>true-文件夹，false-文件</returns>
        public static bool CheckIsDir(string path)
        {
            return string.IsNullOrEmpty(Path.GetExtension(path));
        }

        /// <summary>
        /// 检查文件夹路径正确性，抛异常
        /// </summary>
        public static void CheckIsDirWithException(string path)
        {
            if (!CheckIsDir(path))
            {
                throw new ArgumentException("应该输入正确的文件夹路径", nameof(path));
            }
        }



        /// <summary>
        /// 是否有效的本地文件夹路径
        /// </summary>
        /// <param name="val"></param>
        /// <returns>是，返回true；不是返回false</returns>
        public static bool IsValidLocalFolderPath(string val)
        {
            // 参考：https://www.cnblogs.com/whr2071/p/16084937.html

            Regex regex = new Regex(@"^([a-zA-Z]:\\)([-\u4e00-\u9fa5\w\s.()~!@#$%^&()\[\]{}+=]+\\?)*$");
            Match result = regex.Match(val);
            return result.Success;
        }
    }
}
