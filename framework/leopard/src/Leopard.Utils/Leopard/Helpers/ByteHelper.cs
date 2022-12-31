using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class ByteHelper
    {
        #region Compare  快速比较两个byte数组是否相等

        // 参考：.NET如何快速比较两个byte数组是否相等
        // https://www.cnblogs.com/InCerry/p/dotnet-compare-two-byte-arrays.html

        //为了保持类型安全，默认情况下，C# 不支持指针运算。不过，通过使用 unsafe 关键字，可以定义可使用指针的不安全上下文。
        //在公共语言运行库(CLR) 中，不安全代码是指无法验证的代码。C# 中的不安全代码不一定是危险的，只是其安全性无法由 CLR 进行验证的代码。因此，CLR 只对在完全受信任的程序集中的不安全代码执行操作。如果使用不安全代码，由您负责确保您的代码不会引起安全风险或指针错误。
        //  不安全代码具有下列属性：
        //  #、方法、类型和可被定义为不安全的代码块。
        //  #、在某些情况下，通过移除数组界限检查，不安全代码可提高应用程序的性能。
        //  #、当调用需要指针的本机函数时，需要使用不安全代码。
        //  #、使用不安全代码将引起安全风险和稳定性风险。
        //  #、在 C# 中，为了编译不安全代码，必须用 /unsafe 编译应用程序。


        // 由于指针使用是内存不安全的操作，所以需要使用unsafe关键字
        // 项目文件中也要加入<AllowUnsafeBlocks>true</AllowUnsafeBlocks>来允许unsafe代码

        [DllImport("msvcrt.dll")]   // 需要使用的dll名称
        private static extern unsafe int memcmp(byte* b1, byte* b2, int count);

        /// <summary>
        /// 快速比较两个byte数组是否相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static unsafe bool CompareArray(byte[] x, byte[] y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.Length != y.Length) return false;

            // 在.NET程序的运行中，垃圾回收器可能会整理和压缩内存，这样会导致数组地址变动
            // 所以，我们需要使用fixed关键字，将x和y数组'固定'在内存中，让GC不移动它
            // 更多详情请看 https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/fixed-statement
            fixed (byte* xPtr = x, yPtr = y)
            {
                return memcmp(xPtr, yPtr, x.Length) == 0;
            }
        }

        #endregion

    }
}
