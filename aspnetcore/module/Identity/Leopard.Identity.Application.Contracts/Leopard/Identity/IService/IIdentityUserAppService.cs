using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户服务
	/// </summary>
	public interface IIdentityUserAppService : ICrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, ICrudAppService<IdentityUserDto, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, IApplicationService, IRemoteService
	{
		/// <summary>
		/// 根据用户Id获取角色信息
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <returns></returns>
		Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);
		/// <summary>
		/// 获取用户所有声明
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <returns></returns>
		Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id);
		/// <summary>
		/// 获取用户组织
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <returns></returns>
		Task<List<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);
		/// <summary>
		/// 更新用户角色信息
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);
		/// <summary>
		/// 更新用户声明信息
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input);
		/// <summary>
		/// 更改用户密码
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input);
		/// <summary>
		/// 变更双因素验证
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task ChangeTwoFactorEnabledAsync(Guid id, ChangeTwoFactorEnabledDto input);
		/// <summary>
		/// 加锁用户
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <param name="seconds">时间（秒）</param>
		/// <returns></returns>
		Task LockAsync(Guid id, int seconds);
		/// <summary>
		/// 解锁用户
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <returns></returns>
		Task UnlockAsync(Guid id);
		/// <summary>
		/// 根据用户名查找用户
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		Task<IdentityUserDto> FindByUsernameAsync(string username);
		/// <summary>
		/// 根据email查找用户
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		Task<IdentityUserDto> FindByEmailAsync(string email);
	}
}
