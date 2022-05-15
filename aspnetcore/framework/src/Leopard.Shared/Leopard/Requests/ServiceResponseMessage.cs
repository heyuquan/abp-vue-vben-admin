using JetBrains.Annotations;
using System;

namespace Leopard.Requests
{
    /// <summary>
    /// 服务调用信息返回。信息等级可以使  info，error，warn
    /// </summary>
    public class ServiceResponseMessage
    {
        private ServiceResponseMessage() { }

        /// <summary>
        /// 私有构造函数，只能通过静态方法来创建该实例
        /// </summary>
        /// <param name="code"></param>
        /// <param name="messageLevel"></param>
        /// <param name="content"></param>
        /// <param name="details"></param>
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
        /// [必填] 服务调用返回消息类型级别  
        /// ServiceResponseMessageLevel 枚举的字符串表示
        /// </summary>
        public string MessageLevel { get; private set; }
        /// <summary>
        /// [必填] 信息内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// [选填] 详细信息
        /// </summary>
        public string Details { get; private set; }


        /// <summary>
        /// 创建 INFO ServiceResponseMessage
        /// </summary>
        /// <param name="code">code，选填</param>
        /// <param name="content">[必填] 信息内容</param>
        /// <param name="details">[选填] 详细信息</param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateInfo(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.INFO, content, details);
        }

        /// <summary>
        /// 创建 Warn ServiceResponseMessage
        /// </summary>
        /// <param name="code">code，选填</param>
        /// <param name="content">[必填] 信息内容</param>
        /// <param name="details">[选填] 详细信息</param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateWarn(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.WARN, content, details);
        }

        /// <summary>
        /// 创建 ERROR ServiceResponseMessage
        /// </summary>
        /// <param name="code">code，选填</param>
        /// <param name="content">[必填] 信息内容</param>
        /// <param name="details">[选填] 详细信息</param>
        /// <returns></returns>
        public static ServiceResponseMessage CreateError(string code, [NotNull] string content, string details)
        {
            return InnerCreateMessage(code, ServiceResponseMessageLevel.ERROR, content, details);
        }

        /// <summary>
        /// 创建 ServiceResponseMessage
        /// </summary>
        /// <param name="code">code，选填</param>
        /// <param name="level"></param>
        /// <param name="content">[必填] 信息内容</param>
        /// <param name="details">[选填] 详细信息</param>
        /// <returns></returns>
        private static ServiceResponseMessage InnerCreateMessage(string code, ServiceResponseMessageLevel level, [NotNull] string content, string details)
        {
            if (string.IsNullOrWhiteSpace(code))
                code = String.Empty;

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            return new ServiceResponseMessage(code, level, content, details);
        }
    }
}
