using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
	[Authorize(IdentityPermissions.ClaimTypes.Default)]
	public class IdentityClaimTypeAppService : ApplicationService, IIdentityClaimTypeAppService, IApplicationService, IRemoteService
	{
		protected IdentityClaimTypeManager IdenityClaimTypeManager { get; }

		protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }

		public IdentityClaimTypeAppService(IdentityClaimTypeManager idenityClaimTypeManager, IIdentityClaimTypeRepository identityClaimTypeRepository)
		{
			IdenityClaimTypeManager = idenityClaimTypeManager;
			IdentityClaimTypeRepository = identityClaimTypeRepository;
		}

		public virtual async Task<PagedResultDto<ClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input)
		{
			var count = await IdentityClaimTypeRepository.GetCountAsync(input.Filter);
			List<IdentityClaimType> source = await this.IdentityClaimTypeRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
			return new PagedResultDto<ClaimTypeDto>((long)count, base.ObjectMapper.Map<List<IdentityClaimType>, List<ClaimTypeDto>>(source));
		}

		public virtual async Task<List<ClaimTypeDto>> GetAllListAsync()
		{
			var claimTypes = await IdentityClaimTypeRepository.GetListAsync();
			return this.MapListClaimTypeToListDto(claimTypes);
		}

		public virtual async Task<ClaimTypeDto> GetAsync(Guid id)
		{
			var claimType = await this.IdentityClaimTypeRepository.GetAsync(id);
			return this.MapClaimTypeToDto(claimType);
		}

		[Authorize(IdentityPermissions.ClaimTypes.Create)]
		public virtual async Task<ClaimTypeDto> CreateAsync(CreateClaimTypeDto input)
		{
			var identityClaimType = base.ObjectMapper.Map<CreateClaimTypeDto, IdentityClaimType>(input);
			input.MapExtraPropertiesTo(identityClaimType);
			var claimType = await this.IdenityClaimTypeManager.CreateAsync(identityClaimType);
			return this.MapClaimTypeToDto(claimType);
		}

		[Authorize(IdentityPermissions.ClaimTypes.Update)]
		public virtual async Task<ClaimTypeDto> UpdateAsync(Guid id, UpdateClaimTypeDto input)
		{
			var identityClaimType = await this.IdentityClaimTypeRepository.GetAsync(id);
			base.ObjectMapper.Map<UpdateClaimTypeDto, IdentityClaimType>(input, identityClaimType);
			input.MapExtraPropertiesTo(identityClaimType);
			var claimType = await this.IdenityClaimTypeManager.UpdateAsync(identityClaimType);
			return this.MapClaimTypeToDto(claimType);
		}

		[Authorize(IdentityPermissions.ClaimTypes.Delete)]
		public virtual async Task DeleteAsync(Guid id)
		{
			await this.IdentityClaimTypeRepository.DeleteAsync(id);
		}

		protected virtual ClaimTypeDto MapClaimTypeToDto(IdentityClaimType claimType)
		{
			var claimTypeDto = base.ObjectMapper.Map<IdentityClaimType, ClaimTypeDto>(claimType);
			claimTypeDto.ValueTypeAsString = claimTypeDto.ValueType.ToString();
			return claimTypeDto;
		}

		protected virtual List<ClaimTypeDto> MapListClaimTypeToListDto(List<IdentityClaimType> claimTypes)
		{
			var list = base.ObjectMapper.Map<List<IdentityClaimType>, List<ClaimTypeDto>>(claimTypes);
			foreach (ClaimTypeDto claimTypeDto in list)
			{
				claimTypeDto.ValueTypeAsString = claimTypeDto.ValueType.ToString();
			}
			return list;
		}
	}
}
