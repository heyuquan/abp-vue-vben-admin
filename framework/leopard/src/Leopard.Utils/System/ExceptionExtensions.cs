using System;
using System.Runtime.ExceptionServices;
using System.Text;

namespace System
{
    /// <summary>
    /// 异常 扩展
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 获取原始异常信息（即最内部的InnerException对象）
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetOriginalException();
        }

        /// <summary>
        /// 转换成日志相关信息，如有InnerException，会递归获取错误信息
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="isHideStackTrace">是否隐藏异常堆栈信息</param>
        /// <returns>异常string</returns>
        public static string ToStrMessage(this Exception exception, bool isHideStackTrace = false)
        {
            StringBuilder sb = new StringBuilder();
            string appString = string.Empty;
            while (exception != null)
            {
                sb.AppendLine(string.Format("{0}异常消息：{1}", appString, exception.Message));
                sb.AppendLine(string.Format("{0}异常类型：{1}", appString, exception.GetType().FullName));
                sb.AppendLine(string.Format("{0}异常方法：{1}", appString, (exception.TargetSite == null ? null : exception.TargetSite.Name)));
                sb.AppendLine(string.Format("{0}异常源：{1}", appString, exception.Source));
                if (!isHideStackTrace && exception.StackTrace != null)
                {
                    sb.AppendLine(string.Format("{0}异常堆栈：{1}", appString, exception.StackTrace));
                }
                if (exception.InnerException != null)
                {
                    sb.AppendLine(string.Format("{0}内部异常：", appString));
                    appString += "  ";
                }
                exception = exception.InnerException;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将异常重新抛出
        /// Abp 里面已经定义了 ReThrow ，所以此处为 ReThrow2
        /// </summary>
        public static void ReThrow2(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public static void DebugDump(this Exception ex)
        {
            try
            {
                ex.StackTrace.DebugDump();
                ex.Message.DebugDump();
            }
            catch
            {
            }
        }
    }
}
