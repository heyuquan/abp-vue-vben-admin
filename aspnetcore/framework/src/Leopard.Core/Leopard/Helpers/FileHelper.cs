using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    /// <summary>
    /// 文件相关帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 确保目录存在，不存在，则创建
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

    }
}
