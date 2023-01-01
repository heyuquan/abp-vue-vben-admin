using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.Identity
{
    /// <summary>
    /// 身份标识用户服务
    /// </summary>
    [ControllerName("User")]
    [Route("api/identity/users")]
    [RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    public class IdentityUserController : AbpController, IIdentityUserAppService, ICrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, ICrudAppService<IdentityUserDto, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, IApplicationService, IRemoteService
    {
        protected IIdentityUserAppService UserAppService { get; }

        public IdentityUserController(IIdentityUserAppService userAppService)
        {
            UserAppService = userAppService;
        }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public virtual Task<IdentityUserDto> GetAsync(Guid id)
        {
            return this.UserAppService.GetAsync(id);
        }

        /// <summary>
        /// 根据条件获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            return this.UserAppService.GetListAsync(input);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            return this.UserAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            return this.UserAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return this.UserAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 根据用户Id获取角色信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [Route("{id}/roles")]
        [HttpGet]
        public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            return this.UserAppService.GetRolesAsync(id);
        }

        /// <summary>
        /// 获取用户所有声明
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [Route("{id}/claims")]
        [HttpGet]
        public virtual Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id)
        {
            return this.UserAppService.GetClaimsAsync(id);
        }

        /// <summary>
        /// 获取用户组织
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/organization-units")]
        public virtual Task<List<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            return this.UserAppService.GetOrganizationUnitsAsync(id);
        }

        /// <summary>
        /// 更新用户角色信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/roles")]
        public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            return this.UserAppService.UpdateRolesAsync(id, input);
        }

        /// <summary>
        /// 更新用户声明信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/claims")]
        [HttpPut]
        public virtual Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input)
        {
            return this.UserAppService.UpdateClaimsAsync(id, input);
        }

        /// <summary>
        /// 加锁用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="seconds">时间（秒）</param>
        /// <returns></returns>
        [Route("{id}/lock")]
        [HttpPut]
        public virtual Task LockAsync(Guid id, int seconds)
        {
            return this.UserAppService.LockAsync(id, seconds);
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [Route("{id}/unlock")]
        [HttpPut]
        public virtual Task UnlockAsync(Guid id)
        {
            return this.UserAppService.UnlockAsync(id);
        }

        /// <summary>
        /// 根据用户名查找用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("by-username/{username}")]
        [HttpGet]
        public virtual Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            return this.UserAppService.FindByUsernameAsync(username);
        }

        /// <summary>
        /// 根据email查找用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("by-email/{email}")]
        public virtual Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return this.UserAppService.FindByEmailAsync(email);
        }

        /// <summary>
        /// 更改用户密码
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/change-password")]
        [HttpPut]
        public virtual Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input)
        {
            return this.UserAppService.UpdatePasswordAsync(id, input);
        }

        /// <summary>
        /// 获取指定用户是否启用双因素验证
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/two-factor-enabled")]
        public virtual async Task<bool> GetTwoFactorEnabledAsync(Guid id)
        {
            return await UserAppService.GetTwoFactorEnabledAsync(id);
        }

        /// <summary>
        /// 变更双因素验证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/two-factor-enabled")]
        public virtual async Task ChangeTwoFactorEnabledAsync(Guid id, ChangeTwoFactorEnabledDto input)
        {
            await UserAppService.ChangeTwoFactorEnabledAsync(id, input);
        }
    }
}
