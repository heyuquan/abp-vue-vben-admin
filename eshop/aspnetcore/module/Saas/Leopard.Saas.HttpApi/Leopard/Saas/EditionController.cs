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
	/// 版本服务
	/// </summary>
	[RemoteService(true, Name = "SaasHost")]
	[Controller]
	[Area("saas")]
	[Route("/api/saas/editions")]
	[ControllerName("Edition")]
	public class EditionController : AbpController, IReadOnlyAppService<EditionDto, EditionDto, Guid, GetEditionsInput>, ICreateAppService<EditionDto, EditionCreateDto>, IUpdateAppService<EditionDto, Guid, EditionUpdateDto>, IDeleteAppService<Guid>, ICreateUpdateAppService<EditionDto, Guid, EditionCreateDto, EditionUpdateDto>, ICrudAppService<EditionDto, EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, ICrudAppService<EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, IEditionAppService, IRemoteService, IApplicationService
	{
		protected IEditionAppService Service { get; }

		public EditionController(IEditionAppService service)
		{
			this.Service = service;
		}

		/// <summary>
		/// 获取版本
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpGet]
		public virtual Task<EditionDto> GetAsync(Guid id)
		{
			return this.Service.GetAsync(id);
		}

		/// <summary>
		/// 获取版本列表
		/// </summary>
		[HttpGet]
		public virtual Task<PagedResultDto<EditionDto>> GetListAsync(GetEditionsInput input)
		{
			return this.Service.GetListAsync(input);
		}
		/// <summary>
		/// 创建版本
		/// </summary>
		[HttpPost]
		public virtual Task<EditionDto> CreateAsync(EditionCreateDto input)
		{
			return this.Service.CreateAsync(input);
		}
		/// <summary>
		/// 更新版本
		/// </summary>
		[HttpPut]
		[Route("{id}")]
		public virtual Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
		{
			return this.Service.UpdateAsync(id, input);
		}
		/// <summary>
		/// 删除版本
		/// </summary>
		[Route("{id}")]
		[HttpDelete]
		public virtual Task DeleteAsync(Guid id)
		{
			return this.Service.DeleteAsync(id);
		}

		/// <summary>
		/// 获取版本使用统计计数
		/// </summary>
		[HttpGet]
		[Route("statistics/usage-statistic")]
		public virtual Task<GetEditionUsageStatisticsResult> GetUsageStatistics()
		{
			return this.Service.GetUsageStatistics();
		}
	}
}
