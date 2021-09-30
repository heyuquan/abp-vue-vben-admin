using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Leopard.Saas.Dtos;

namespace Leopard.Saas
{
	/// <summary>
	/// 租户服务
	/// </summary>
	[Controller]
	[Area("saas")]
	[ControllerName("Tenant")]
	[Route("/api/saas/tenants")]
	[RemoteService(true, Name = "SaasHost")]
	public class TenantController : AbpController, IDeleteAppService<Guid>, IRemoteService, IApplicationService, IReadOnlyAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput>, ICreateAppService<SaasTenantDto, SaasTenantCreateDto>, IUpdateAppService<SaasTenantDto, Guid, SaasTenantUpdateDto>, ICreateUpdateAppService<SaasTenantDto, Guid, SaasTenantCreateDto, SaasTenantUpdateDto>, ICrudAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ICrudAppService<SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ITenantAppService
	{
		protected ITenantAppService Service { get; }

		public TenantController(ITenantAppService service)
		{
			this.Service = service;
		}

		/// <summary>
		/// 获取租户
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}")]
		public virtual Task<SaasTenantDto> GetAsync(Guid id)
		{
			return this.Service.GetAsync(id);
		}
		/// <summary>
		/// 获取租户列表
		/// </summary>
		[HttpGet]
		public virtual Task<PagedResultDto<SaasTenantDto>> GetListAsync(GetTenantsInput input)
		{
			return this.Service.GetListAsync(input);
		}
		/// <summary>
		/// 创建租户
		/// </summary>
		[HttpPost]
		public virtual Task<SaasTenantDto> CreateAsync(SaasTenantCreateDto input)
		{
			this.ValidateModel();
			return this.Service.CreateAsync(input);
		}
		/// <summary>
		/// 更新租户
		/// </summary>
		[HttpPut]
		[Route("{id}")]
		public virtual Task<SaasTenantDto> UpdateAsync(Guid id, SaasTenantUpdateDto input)
		{
			return this.Service.UpdateAsync(id, input);
		}
		/// <summary>
		/// 删除租户
		/// </summary>
		[HttpDelete]
		[Route("{id}")]
		public virtual Task DeleteAsync(Guid id)
		{
			return this.Service.DeleteAsync(id);
		}

		/// <summary>
		/// 获取租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <returns></returns>
		[Route("{id}/default-connection-string")]
		[HttpGet]
		public virtual Task<string> GetDefaultConnectionStringAsync(Guid id)
		{
			return this.Service.GetDefaultConnectionStringAsync(id);
		}
		/// <summary>
		/// 更新租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <param name="defaultConnectionString"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("{id}/default-connection-string")]
		public virtual Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
		{
			return this.Service.UpdateDefaultConnectionStringAsync(id, defaultConnectionString);
		}
		/// <summary>
		/// 删除租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id}/default-connection-string")]
		public virtual Task DeleteDefaultConnectionStringAsync(Guid id)
		{
			return this.Service.DeleteDefaultConnectionStringAsync(id);
		}
	}
}
