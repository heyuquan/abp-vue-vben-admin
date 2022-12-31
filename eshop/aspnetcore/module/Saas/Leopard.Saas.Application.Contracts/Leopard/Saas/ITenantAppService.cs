using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Leopard.Saas.Dtos;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Leopard.Saas
{
	/// <summary>
	/// 租户服务
	/// </summary>
	public interface ITenantAppService : IDeleteAppService<Guid>, IUpdateAppService<SaasTenantDto, Guid, SaasTenantUpdateDto>, ICreateAppService<SaasTenantDto, SaasTenantCreateDto>, ICreateUpdateAppService<SaasTenantDto, Guid, SaasTenantCreateDto, SaasTenantUpdateDto>, IReadOnlyAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput>, ICrudAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ICrudAppService<SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, IRemoteService, IApplicationService
	{
		/// <summary>
		/// 获取指定租户的字符串连接列表
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringListAsync(Guid id);
		/// <summary>
		/// 获取指定租户，指定名称的字符串连接
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name">字符串名称</param>
		/// <returns></returns>
		Task<TenantConnectionStringDto> GetConnectionStringAsync(Guid id, string name);
		/// <summary>
		/// 更新指定租户的字符串连接
		/// </summary>
		/// <param name="id"></param>
		/// <param name="dto"></param>
		/// <returns></returns>
		Task UpdateConnectionStringAsync(Guid id, TenantConnectionStringUpdateDto dto);
		/// <summary>
		/// 删除指定租户，指定名称的字符串连接
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name">字符串名称</param>
		/// <returns></returns>
		Task DeleteConnectionStringAsync(Guid id, string name);
	}
}
