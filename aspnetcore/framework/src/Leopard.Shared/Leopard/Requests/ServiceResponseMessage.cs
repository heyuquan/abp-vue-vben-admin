using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Requests
{
    /// <summary>
    /// 服务调用信息返回。信息等级可以使  info，error，warn
    /// </summary>
    public class ServiceResponseMessage
    {
        private ServiceResponseMessage() { }

        private ServiceResponseMessage(string code, ServiceResponseMessageLevel messageLevel, string content, string details)
        {
            Code = code;
            MessageLevel = messageLevel.GetEnumMemberValue();
            Content = content;
            Details = details;
        }

        /// <summary>
        /// [选填] 每个信息都对应一个编码。方便用code查对应信息的解决方案
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 服务调用返回消息类型级别  
        /// ServiceResponseMessageLevel 枚举的字符串表示
        /// </summary>
        public string MessageLevel { get; private set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Details { get; private set; }


        /// <summary>
        /// 创建 INFO ServiceResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateInfo(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.INFO, content, details);
        }

        /// <summary>
        /// 创建 Warn ServiceResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateWarn(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.WARN, content, details);
        }

        /// <summary>
        /// 创建 ERROR ServiceResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateError(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.ERROR, content, details);
        }

        /// <summary>
        /// 创建 ServiceResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="level"></param>
        /// <param name="content"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private static ServiceResponseMessage InnerCreateMessage(string code, ServiceResponseMessageLevel level, [NotNull] string content, string details)
        {
            if (code.IsNullOrWhiteSpace())
                code = String.Empty;

            if (content.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(content));

            return new ServiceResponseMessage(code, level, content, details);
        }
    }
}
