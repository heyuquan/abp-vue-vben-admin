using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Leopard
{
    /// <summary>
    /// 系统相关
    /// </summary>
    public static partial class SystemHelper
    {
        /// <summary>
        /// 判断应用程序是否以管理员身份运行（支持windows、linux）
        /// </summary>
        public static bool IsAdministrator()
        {
            // 需要引用nuget包Mono.Posix.NETStandard
            // 参考：https://mp.weixin.qq.com/s/iFT43Kzuys5euHWL1swKsA
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                  new WindowsPrincipal(WindowsIdentity.GetCurrent())
                      .IsInRole(WindowsBuiltInRole.Administrator) :
                  Mono.Unix.Native.Syscall.geteuid() == 0;
        }
    }
}
