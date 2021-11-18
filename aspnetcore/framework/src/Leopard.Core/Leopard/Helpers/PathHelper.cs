using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    /// <summary>
    /// 路径帮助类
    /// </summary>
    public class PathHelper
    {
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
    }
}
