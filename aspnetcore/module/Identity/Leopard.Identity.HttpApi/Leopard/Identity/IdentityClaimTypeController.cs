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
	/// 身份标识声明类型服务
	/// </summary>
	[Route("api/identity/claim-types")]
	[RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
	[ControllerName("ClaimType")]
	[Area("identity")]
	public class IdentityClaimTypeController : AbpController, IIdentityClaimTypeAppService, IApplicationService, IRemoteService
	{
		protected IIdentityClaimTypeAppService ClaimTypeAppService { get; }

		public IdentityClaimTypeController(IIdentityClaimTypeAppService claimTypeAppService)
		{
			ClaimTypeAppService = claimTypeAppService;
		}

		/// <summary>
		/// 根据条件获取声明类型列表
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpGet]
		public virtual Task<PagedResultDto<ClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input)
		{
			return this.ClaimTypeAppService.GetListAsync(input);
		}

		/// <summary>
		/// 获取所有申明类型
		/// </summary>
		[Route("all")]
		[HttpGet]
		public virtual Task<List<ClaimTypeDto>> GetAllListAsync()
		{
			return this.ClaimTypeAppService.GetAllListAsync();
		}

		/// <summary>
		/// 获取指定id的声明类型
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpGet]
		public virtual Task<ClaimTypeDto> GetAsync(Guid id)
		{
			return this.ClaimTypeAppService.GetAsync(id);
		}

	    /// <summary>
		/// 创建声明类型
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPost]
		public virtual Task<ClaimTypeDto> CreateAsync(CreateClaimTypeDto input)
		{
			return this.ClaimTypeAppService.CreateAsync(input);
		}

		/// <summary>
		/// 更新声明类型
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("{id}")]
		public virtual Task<ClaimTypeDto> UpdateAsync(Guid id, UpdateClaimTypeDto input)
		{
			return this.ClaimTypeAppService.UpdateAsync(id, input);
		}

		/// <summary>
		/// 删除声明类型
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpDelete]
		public virtual Task DeleteAsync(Guid id)
		{
			return this.ClaimTypeAppService.DeleteAsync(id);
		}
	}
}
