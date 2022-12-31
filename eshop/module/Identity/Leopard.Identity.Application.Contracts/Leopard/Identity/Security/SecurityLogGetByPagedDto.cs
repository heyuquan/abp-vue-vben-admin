using System;
using Volo.Abp.Application.Dtos;

namespace Leopard.Identity.Security
{
    /// <summary>
    /// 获取分页安全日志Dto
    /// </summary>
    public class SecurityLogGetByPagedDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
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
        public string ActionName { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 跟踪Id
        /// </summary>
        public string CorrelationId { get; set; }
    }
}
