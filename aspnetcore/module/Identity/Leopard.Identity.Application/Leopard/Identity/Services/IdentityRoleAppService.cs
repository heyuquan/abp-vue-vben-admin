using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
	[Authorize(IdentityPermissions.Roles.Default)]
	public class IdentityRoleAppService : IdentityAppServiceBase, IIdentityRoleAppService, IApplicationService, IRemoteService
	{
		protected IdentityRoleManager RoleManager { get; }

		protected IIdentityRoleRepository RoleRepository { get; }

		public IdentityRoleAppService(IdentityRoleManager roleManager, IIdentityRoleRepository roleRepository)
		{
			RoleManager = roleManager;
			RoleRepository = roleRepository;
		}

		public virtual async Task<IdentityRoleDto> GetAsync(Guid id)
		{
			IdentityRole source = await this.RoleManager.GetByIdAsync(id);
			return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(source);
		}

		public virtual async Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
		{
			List<IdentityRole> source = await this.RoleRepository.GetListAsync();
			return new ListResultDto<IdentityRoleDto>(base.ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(source));
		}

		public virtual async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRoleListInput input)
		{
			var list = await RoleRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
			var totalCount = await RoleRepository.GetCountAsync(input.Filter);

			return new PagedResultDto<IdentityRoleDto>(
				totalCount,
				ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list)
				);
		}

		[Authorize(IdentityPermissions.Roles.Update)]
		public virtual async Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaimDto> input)
		{
			var role = await this.RoleRepository.GetAsync(id);
			foreach (var roleClaim in input)
			{
				if (role.FindClaim(new Claim(roleClaim.ClaimType, roleClaim.ClaimValue)) == null)
				{
					role.AddClaim(base.GuidGenerator, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
				}
			}

			var list = role.Claims.ToList<IdentityRoleClaim>();
			foreach(var claim in list)
			{
				if (!input.Any((IdentityRoleClaimDto c) => claim.ClaimType == c.ClaimType && claim.ClaimValue == c.ClaimValue))
				{
					role.RemoveClaim(new Claim(claim.ClaimType, claim.ClaimValue));
				}
			}
		}

		[Authorize(IdentityPermissions.Roles.Default)]
		public virtual async Task<List<IdentityRoleClaimDto>> GetClaimsAsync(Guid id)
		{
			IdentityRole identityRole = await this.RoleRepository.GetAsync(id);
			return new List<IdentityRoleClaimDto>(base.ObjectMapper.Map<List<IdentityRoleClaim>, List<IdentityRoleClaimDto>>(identityRole.Claims.ToList<IdentityRoleClaim>()));
		}

		[Authorize(IdentityPermissions.Roles.Create)]
		public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
		{
			var role = new IdentityRole(
				GuidGenerator.Create(),
				input.Name,
				CurrentTenant.Id
			)
			{
				IsDefault = input.IsDefault,
				IsPublic = input.IsPublic
			};

			input.MapExtraPropertiesTo(role);

			(await RoleManager.CreateAsync(role)).CheckErrors();
			await CurrentUnitOfWork.SaveChangesAsync();

			return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
		}

		[Authorize(IdentityPermissions.Roles.Update)]
		public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
		{
			var role = await RoleManager.GetByIdAsync(id);
			role.ConcurrencyStamp = input.ConcurrencyStamp;

			(await RoleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();

			role.IsDefault = input.IsDefault;
			role.IsPublic = input.IsPublic;

			input.MapExtraPropertiesTo(role);

			(await RoleManager.UpdateAsync(role)).CheckErrors();
			await CurrentUnitOfWork.SaveChangesAsync();

			return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
		}

		[Authorize(IdentityPermissions.Roles.Delete)]
		public virtual async Task DeleteAsync(Guid id)
		{
			IdentityRole identityRole = await this.RoleManager.FindByIdAsync(id.ToString());
			if (identityRole != null)
			{
				(await this.RoleManager.DeleteAsync(identityRole)).CheckErrors();
			}
		}
	}
}
