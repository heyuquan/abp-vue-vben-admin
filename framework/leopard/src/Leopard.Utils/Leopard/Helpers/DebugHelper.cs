using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    /// <summary>
    /// 调试帮助类
    /// </summary>
    public static class DebugHelper
    {
        // 我不想再传递 nameof 了
        // 下次调试了，可以看看这里面的方式
        // https://www.cnblogs.com/newbe36524/p/0x01D-I-don-t-want-to-pass-nameof-anymore.html

        /// <summary>
        /// 获取调用者的方法名、所在文件、行号、列号等信息
        /// </summary>
        /// <returns></returns>
        public static string GetCaller()
        {
            // 第一帧是 GetCaller本身，所以跳过；fNeedFileInfo设置成 true，否则调用者所在文件等信息会为空。

            StackTrace st = new StackTrace(skipFrames: 1, fNeedFileInfo: true);
            StackFrame[] sfArray = st.GetFrames();

            return string.Join(" -> ",
                 sfArray.Select(r =>
                     $"{r.GetMethod().Name} in {r.GetFileName()} line:{r.GetFileLineNumber()} column:{r.GetFileColumnNumber()}"));

        }
    }
}
