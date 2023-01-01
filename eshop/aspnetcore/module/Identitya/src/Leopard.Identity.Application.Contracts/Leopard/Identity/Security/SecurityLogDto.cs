using System;
using Volo.Abp.Application.Dtos;

namespace Leopard.Identity.Security
{
    /// <summary>
    /// 安全日志
    /// </summary>
    public class SecurityLogDto : ExtensibleEntityDto<Guid>
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 身份标识
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 操作名
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 租户名
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 跟踪Id
        /// </summary>
        public string CorrelationId { get; set; }
        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
