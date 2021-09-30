using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectExtending;
using Leopard.Saas.Dtos;
using Leopard.Saas.Permissions;

namespace Leopard.Saas
{
	[Authorize(SaasPermissions.Editions.Default)]
	public class EditionAppService : SaasAppServiceBase, IDeleteAppService<Guid>, IUpdateAppService<EditionDto, Guid, EditionUpdateDto>, ICreateAppService<EditionDto, EditionCreateDto>, ICreateUpdateAppService<EditionDto, Guid, EditionCreateDto, EditionUpdateDto>, IReadOnlyAppService<EditionDto, EditionDto, Guid, GetEditionsInput>, ICrudAppService<EditionDto, EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, ICrudAppService<EditionDto, Guid, GetEditionsInput, EditionCreateDto, EditionUpdateDto>, IRemoteService, IApplicationService, IEditionAppService
	{
		protected IEditionRepository EditionRepository { get; }

		protected ITenantRepository TenantRepository { get; }

		public EditionAppService(IEditionRepository editionRepository, ITenantRepository tenantRepository)
		{
			this.EditionRepository = editionRepository;
			this.TenantRepository = tenantRepository;
		}

		public virtual async Task<EditionDto> GetAsync(Guid id)
		{
			var edition = await this.EditionRepository.GetAsync(id);
			return base.ObjectMapper.Map<Edition, EditionDto>(edition);
		}

		public virtual async Task<PagedResultDto<EditionDto>> GetListAsync(GetEditionsInput input)
		{
			int count = await this.EditionRepository.GetCountAsync(input.Filter);
			var list = await this.EditionRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
			return new PagedResultDto<EditionDto>((long)count, base.ObjectMapper.Map<List<Edition>, List<EditionDto>>(list));
		}

		[Authorize(SaasPermissions.Editions.Create)]
		public virtual async Task<EditionDto> CreateAsync(EditionCreateDto input)
		{
			var edition = await this.EditionRepository.InsertAsync(new Edition(GuidGenerator.Create(), input.DisplayName));
			return ObjectMapper.Map<Edition, EditionDto>(edition);
		}

		[Authorize(SaasPermissions.Editions.Update)]
		public virtual async Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
		{
			var edition = await this.EditionRepository.GetAsync(id);
			edition.SetDisplayName(input.DisplayName);
			input.MapExtraPropertiesTo(edition);
			var source = await this.EditionRepository.UpdateAsync(edition);
			return base.ObjectMapper.Map<Edition, EditionDto>(source);
		}

		[Authorize(SaasPermissions.Editions.Delete)]
		public virtual async Task DeleteAsync(Guid id)
		{
			await this.EditionRepository.DeleteAsync(id);
		}

		public virtual async Task<GetEditionUsageStatisticsResult> GetUsageStatistics()
		{
			List<Edition> editions = await this.EditionRepository.GetListAsync();
			var list = from info in await this.TenantRepository.GetListAsync()
					   group info by info.EditionId into @group
					   select new
					   {
						   EditionId = @group.Key,
						   Count = @group.Count<Tenant>()
					   };
			var dictionary = new Dictionary<string, int>();
			foreach (var x in list)
			{
				var edition = editions.FirstOrDefault(e => e.Id == x.EditionId);
				string text = (edition != null) ? edition.DisplayName : null;
				if (text != null)
				{
					dictionary.Add(text, x.Count);
				}
			}
			return new GetEditionUsageStatisticsResult
			{
				Data = dictionary
			};
		}
	}
}
