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
	/// 身份标识角色服务
	/// </summary>
	[ControllerName("Role")]
	[Route("api/identity/roles")]
	[RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
	[Area("identity")]
	public class IdentityRoleController : AbpController, IIdentityRoleAppService, IApplicationService, IRemoteService
	{
		protected IIdentityRoleAppService RoleAppService { get; }

		public IdentityRoleController(IIdentityRoleAppService roleAppService)
		{
			RoleAppService = roleAppService;
		}

		/// <summary>
		/// 根据id获取角色
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpGet]
		public virtual Task<IdentityRoleDto> GetAsync(Guid id)
		{
			return this.RoleAppService.GetAsync(id);
		}

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPost]
		public virtual Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
		{
			return this.RoleAppService.CreateAsync(input);
		}

		/// <summary>
		/// 更新角色
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpPut]
		public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
		{
			return this.RoleAppService.UpdateAsync(id, input);
		}

		/// <summary>
		/// 删除角色
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id}")]
		public virtual Task DeleteAsync(Guid id)
		{
			return this.RoleAppService.DeleteAsync(id);
		}

		/// <summary>
		/// 获取所有角色
		/// </summary>
		/// <returns></returns>
		[Route("all")]
		[HttpGet]
		public virtual Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
		{
			return this.RoleAppService.GetAllListAsync();
		}

		/// <summary>
		/// 根据条件获取角色列表
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpGet]
		public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRoleListInput input)
		{
			return this.RoleAppService.GetListAsync(input);
		}

		///// <summary>
		///// 更新角色声明
		///// </summary>
		///// <param name="id"></param>
		///// <param name="input"></param>
		///// <returns></returns>
		//[HttpPut]
		//[Route("{id}/claims")]
		//public virtual Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaimDto> input)
		//{
		//	return this.RoleAppService.UpdateClaimsAsync(id, input);
		//}

		/// <summary>
		/// 获取角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}/claims")]
		public virtual Task<List<IdentityRoleClaimDto>> GetClaimsAsync(Guid id)
		{
			return this.RoleAppService.GetClaimsAsync(id);
		}

		/// <summary>
		/// 添加角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("{id}/claims")]
		public virtual Task AddClaimAsync(Guid id, IdentityRoleClaimCreateInput input)
		{
			return this.RoleAppService.AddClaimAsync(id, input);
		}
		/// <summary>
		/// 更新角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("{id}/claims")]
		public virtual Task UpdateClaimAsync(Guid id, IdentityRoleClaimUpdateInput input)
		{
			return this.RoleAppService.UpdateClaimAsync(id, input);
		}
		/// <summary>
		/// 删除角色声明
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id}/claims")]
		public virtual Task DeleteClaimAsync(Guid id, IdentityRoleClaimDeleteInput input)
		{
			return this.RoleAppService.DeleteClaimAsync(id, input);
		}
	}
}
