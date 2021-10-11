using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Leopard.Saas.Dtos;

namespace Leopard.Saas
{
	/// <summary>
	/// 租户服务
	/// </summary>
	public interface ITenantAppService : IDeleteAppService<Guid>, IUpdateAppService<SaasTenantDto, Guid, SaasTenantUpdateDto>, ICreateAppService<SaasTenantDto, SaasTenantCreateDto>, ICreateUpdateAppService<SaasTenantDto, Guid, SaasTenantCreateDto, SaasTenantUpdateDto>, IReadOnlyAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput>, ICrudAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ICrudAppService<SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, IRemoteService, IApplicationService
	{
		/// <summary>
		/// 获取租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <returns></returns>
		Task<string> GetDefaultConnectionStringAsync(Guid id);
		/// <summary>
		/// 更新租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <param name="defaultConnectionString"></param>
		/// <returns></returns>
		Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);
		/// <summary>
		/// 删除租户默认链接字符串
		/// </summary>
		/// <param name="id">租户Id</param>
		/// <returns></returns>
		Task DeleteDefaultConnectionStringAsync(Guid id);
	}
}
