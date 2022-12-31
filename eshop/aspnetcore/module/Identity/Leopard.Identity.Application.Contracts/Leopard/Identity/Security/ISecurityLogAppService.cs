using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Leopard.Identity.Security
{
    /// <summary>
    /// 安全日志服务
    /// </summary>
    public interface ISecurityLogAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件获取安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetByPagedDto input);
        /// <summary>
        /// 根据Id获取安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SecurityLogDto> GetAsync(Guid id);
        /// <summary>
        /// 删除安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
    }
}
