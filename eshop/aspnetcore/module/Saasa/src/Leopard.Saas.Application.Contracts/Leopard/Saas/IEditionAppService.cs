using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Leopard.Saas.Dtos;

namespace Leopard.Saas
{
	/// <summary>
	/// 版本服务
	/// </summary>
	public interface IEditionAppService : IDeleteAppService<Guid>, IUpdateAppService<EditionDto, Guid, EditionUpdateDto>, ICreateAppService<EditionDto, EditionCreateDto>, ICreateUpdateAppService<EditionDto, Guid, EditionCreateDto, EditionUpdateDto>, IReadOnlyAppService<EditionDto, EditionDto, Guid, GetEditionsInput>, ICrudAppService<EditionDto, EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, ICrudAppService<EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, IRemoteService, IApplicationService
	{
		/// <summary>
		/// 获取版本使用统计计数
		/// </summary>
		Task<GetEditionUsageStatisticsResult> GetUsageStatistics();
	}
}
