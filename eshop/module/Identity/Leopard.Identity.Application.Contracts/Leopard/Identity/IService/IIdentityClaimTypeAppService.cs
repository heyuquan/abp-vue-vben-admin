using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识声明类型服务
	/// </summary>
	public interface IIdentityClaimTypeAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 根据条件获取声明类型列表
		/// </summary>
		Task<PagedResultDto<ClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input);
		/// <summary>
		/// 获取所有申明类型
		/// </summary>
		Task<List<ClaimTypeDto>> GetAllListAsync();
		/// <summary>
		/// 获取指定Id的申明类型
		/// </summary>
		Task<ClaimTypeDto> GetAsync(Guid id);
		/// <summary>
		/// 创建申明类型
		/// </summary>
		Task<ClaimTypeDto> CreateAsync(CreateClaimTypeDto input);
		/// <summary>
		/// 更新申明类型
		/// </summary>
		Task<ClaimTypeDto> UpdateAsync(Guid id, UpdateClaimTypeDto input);
		/// <summary>
		/// 删除申明类型
		/// </summary>
		Task DeleteAsync(Guid id);
	}
}
