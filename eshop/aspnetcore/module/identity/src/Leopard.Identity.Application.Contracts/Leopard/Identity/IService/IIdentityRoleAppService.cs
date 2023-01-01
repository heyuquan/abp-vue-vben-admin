using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识角色服务
	/// </summary>
	public interface IIdentityRoleAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 获取所有角色
		/// </summary>
		/// <returns></returns>
		Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
		/// <summary>
		/// 根据条件获取角色列表
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRoleListInput input);
		///// <summary>
		///// 更新角色声明
		///// </summary>
		///// <param name="id"></param>
		///// <param name="input"></param>
		///// <returns></returns>
		//Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaimDto> input);
		/// <summary>
		/// 获取角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<IdentityRoleClaimDto>> GetClaimsAsync(Guid id);
		/// <summary>
		/// 添加角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task AddClaimAsync(Guid id, IdentityRoleClaimCreateInput input);
		/// <summary>
		/// 更新角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task UpdateClaimAsync(Guid id, IdentityRoleClaimUpdateInput input);
		/// <summary>
		/// 删除角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task DeleteClaimAsync(Guid id, IdentityRoleClaimDeleteInput input);

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input);
		/// <summary>
		/// 根据id获取角色
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<IdentityRoleDto> GetAsync(Guid id);
		/// <summary>
		/// 更新角色
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input);
		/// <summary>
		/// 删除角色
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteAsync(Guid id);
	}
}
