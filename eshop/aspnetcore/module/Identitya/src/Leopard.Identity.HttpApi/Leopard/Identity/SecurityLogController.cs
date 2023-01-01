using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Leopard.Identity;
using Leopard.Identity.Security;

namespace Leopard.AuditLogging.Security
{
    [RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
    [Area("auditing")]
    [ControllerName("security-log")]
    [Route("api/identity/security-log")]
    public class SecurityLogController : AbpController, ISecurityLogAppService
    {
        protected ISecurityLogAppService SecurityLogAppService { get; }

        public SecurityLogController(ISecurityLogAppService securityLogAppService)
        {
            SecurityLogAppService = securityLogAppService;
        }
        /// <summary>
        /// 删除安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await SecurityLogAppService.DeleteAsync(id);
        }
        /// <summary>
        /// 根据Id获取安全日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual async Task<SecurityLogDto> GetAsync(Guid id)
        {
            return await SecurityLogAppService.GetAsync(id);
        }
        /// <summary>
        /// 根据条件获取安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetByPagedDto input)
        {
            return await SecurityLogAppService.GetListAsync(input);
        }
    }
}
