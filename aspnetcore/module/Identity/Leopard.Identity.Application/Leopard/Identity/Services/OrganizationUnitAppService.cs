using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
    [Authorize(IdentityPermissions.OrganizationUnits.Default)]
    public class OrganizationUnitAppService : IdentityAppServiceBase, IOrganizationUnitAppService, IApplicationService, IRemoteService
    {
        protected OrganizationUnitManager OrganizationUnitManager { get; }

        protected IdentityUserManager UserManager { get; }

        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

        protected IIdentityUserRepository IdentityUserRepository { get; }

        protected IIdentityRoleRepository IdentityRoleRepository { get; }

        public OrganizationUnitAppService(OrganizationUnitManager organizationUnitManager, IdentityUserManager userManager, IOrganizationUnitRepository organizationUnitRepository, IIdentityUserRepository identityUserRepository, IIdentityRoleRepository identityRoleRepository)
        {
            OrganizationUnitManager = organizationUnitManager;
            UserManager = userManager;
            OrganizationUnitRepository = organizationUnitRepository;
            IdentityUserRepository = identityUserRepository;
            IdentityRoleRepository = identityRoleRepository;
        }

        public virtual async Task<OrganizationUnitWithDetailsDto> GetAsync(Guid id)
        {
            var ou = await this.OrganizationUnitRepository.GetAsync(id);
            var roles = await this.getRoles(new OrganizationUnit[] { ou });
            var result = await this.getOuWithRoles(ou, roles);
            return result;
        }

        public virtual async Task<PagedResultDto<OrganizationUnitWithDetailsDto>> GetListAsync(GetOrganizationUnitInput input)
        {
            var list = new List<OrganizationUnitWithDetailsDto>();

            var organizationUnits = await OrganizationUnitRepository.GetListAsync();
            foreach (var ou in organizationUnits.Where(x => !x.DisplayName.Equals(input.Filter)))
            {
                var organizationUnitWithDetailsDto = new OrganizationUnitWithDetailsDto()
                {
                    Id = ou.Id,
                    ParentId = ou.ParentId,
                    Code = ou.Code,
                    DisplayName = ou.DisplayName,
                    Roles = ObjectMapper.Map<List<OrganizationUnitRole>, List<IdentityRoleDto>>(ou.Roles?.ToList()),
                };

                list.Add(organizationUnitWithDetailsDto);
            }

            return new PagedResultDto<OrganizationUnitWithDetailsDto>(
                organizationUnits.Count,
                list
                );
        }

        public virtual async Task<ListResultDto<OrganizationUnitWithDetailsDto>> GetListAllAsync()
        {
            var list = new List<OrganizationUnitWithDetailsDto>();

            var organizationUnits = await OrganizationUnitRepository.GetListAsync();
            foreach (var ou in organizationUnits)
            {
                var organizationUnitWithDetailsDto = new OrganizationUnitWithDetailsDto()
                {
                    Id = ou.Id,
                    ParentId = ou.ParentId,
                    Code = ou.Code,
                    DisplayName = ou.DisplayName,
                    Roles = ObjectMapper.Map<List<OrganizationUnitRole>, List<IdentityRoleDto>>(ou.Roles?.ToList()),
                };

                list.Add(organizationUnitWithDetailsDto);
            }

            return new ListResultDto<OrganizationUnitWithDetailsDto>(list);
        }

        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            var list = await OrganizationUnitRepository.GetRolesAsync(organizationUnit, input.Sorting, input.MaxResultCount, input.SkipCount);
            var count = await OrganizationUnitRepository.GetRolesCountAsync(organizationUnit);

            return new PagedResultDto<IdentityRoleDto>(
                count,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list)
                );
        }

        public virtual async Task<PagedResultDto<IdentityUserDto>> GetMembersAsync(Guid id, GetIdentityUsersInput input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            var list = await OrganizationUnitRepository.GetMembersAsync(organizationUnit, input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
            var count = await OrganizationUnitRepository.GetMembersCountAsync(organizationUnit, input.Filter);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
                );
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageOU)]
        public virtual async Task<OrganizationUnitWithDetailsDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            var organizationUnit = new OrganizationUnit(GuidGenerator.Create(), input.DisplayName, input.ParentId, CurrentTenant.Id);

            await OrganizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitWithDetailsDto>(organizationUnit);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageOU)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var flag = OrganizationUnitRepository.FindAsync(id);

            if (flag != null)
            {
                await this.OrganizationUnitManager.DeleteAsync(id);
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageOU)]
        public virtual async Task<OrganizationUnitWithDetailsDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            var ou = await OrganizationUnitRepository.FindAsync(id);

            input.MapExtraPropertiesTo(ou);

            await OrganizationUnitManager.UpdateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitWithDetailsDto>(ou);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task AddRolesAsync(Guid id, OrganizationUnitRoleInput input)
        {
            foreach (var roleId in input.RoleIds)
            {
                await this.OrganizationUnitManager.AddRoleToOrganizationUnitAsync(roleId, id);
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task AddMembersAsync(Guid id, OrganizationUnitUserInput input)
        {
            foreach (var userId in input.UserIds)
            {
                await this.UserManager.AddToOrganizationUnitAsync(userId, id);
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageOU)]
        public virtual async Task MoveAsync(Guid id, OrganizationUnitMoveInput input)
        {
            await this.OrganizationUnitManager.MoveAsync(id, input.NewParentId);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task RemoveMemberAsync(Guid id, Guid memberId)
        {
            await this.UserManager.RemoveFromOrganizationUnitAsync(memberId, id);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task RemoveRoleAsync(Guid id, Guid roleId)
        {
            await this.OrganizationUnitManager.RemoveRoleFromOrganizationUnitAsync(roleId, id);
        }

        private async Task<OrganizationUnitWithDetailsDto> getOuWithRoles(OrganizationUnit ou, Dictionary<Guid, IdentityRole> roles)
        {
            var organizationUnitWithDetailsDto = base.ObjectMapper.Map<OrganizationUnit, OrganizationUnitWithDetailsDto>(ou);
            organizationUnitWithDetailsDto.Roles = new List<IdentityRoleDto>();
            foreach (OrganizationUnitRole organizationUnitRole in ou.Roles)
            {
                var role = roles.GetOrDefault(organizationUnitRole.RoleId);
                if (role != null)
                {
                    organizationUnitWithDetailsDto.Roles.Add(base.ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role));
                }
            }
            return await Task.FromResult<OrganizationUnitWithDetailsDto>(organizationUnitWithDetailsDto);
        }

        private async Task<Dictionary<Guid, IdentityRole>> getRoles(IEnumerable<OrganizationUnit> ou)
        {
            Guid[] ids = (from t in ou.SelectMany((OrganizationUnit q) => q.Roles)
                          select t.RoleId).Distinct<Guid>().ToArray<Guid>();
            return (await this.IdentityRoleRepository.GetListAsync(ids)).ToDictionary((IdentityRole u) => u.Id, (IdentityRole u) => u);
        }
    }
}
