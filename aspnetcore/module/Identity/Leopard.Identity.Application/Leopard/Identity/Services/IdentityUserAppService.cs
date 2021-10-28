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
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService, ICrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, ICrudAppService<IdentityUserDto, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>, IApplicationService, IRemoteService
    {
        protected IdentityUserManager UserManager { get; }

        protected IIdentityUserRepository UserRepository { get; }

        public IdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository)
        {
            UserManager = userManager;
            UserRepository = userRepository;
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            IdentityUser source = await this.UserManager.GetByIdAsync(id);
            IdentityUserDto result = ObjectMapper.Map<IdentityUser, IdentityUserDto>(source);
            return result;
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await UserRepository.GetCountAsync(input.Filter);
            var list = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            List<IdentityUserDto> list2 = base.ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list);
            for (int i = 0; i < list.Count; i++)
            {
                list2[i].IsLockedOut = (list[i].LockoutEnabled && list[i].LockoutEnd != null && list[i].LockoutEnd > DateTime.UtcNow);
            }
            return new PagedResultDto<IdentityUserDto>(count, list2);
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await UserRepository.GetRolesAsync(id);
            return new ListResultDto<IdentityRoleDto>(ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles));
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<List<IdentityUserClaimDto>> GetClaimsAsync(Guid id)
        {
            IdentityUser identityUser = await UserRepository.GetAsync(id);
            return new List<IdentityUserClaimDto>(ObjectMapper.Map<List<IdentityUserClaim>, List<IdentityUserClaimDto>>(identityUser.Claims.ToList<IdentityUserClaim>()));
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<List<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            List<OrganizationUnit> source = await UserRepository.GetOrganizationUnitsAsync(id);
            return new List<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(source));
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            input.MapExtraPropertiesTo(user);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
            await UpdateUserByInput(user, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await UserManager.GetByIdAsync(id);
            user.ConcurrencyStamp = input.ConcurrencyStamp;

            (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

            await UpdateUserByInput(user, input);
            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            if (CurrentUser.Id == id)
            {
                throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
            }

            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            (await UserManager.DeleteAsync(user)).CheckErrors();
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            var user = await UserManager.GetByIdAsync(id);
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            await UserRepository.UpdateAsync(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdateClaimsAsync(Guid id, List<IdentityUserClaimDto> input)
        {
            var user = await this.UserRepository.GetAsync(id);
            foreach (var userClaim in input)
            {
                if (user.FindClaim(new Claim(userClaim.ClaimType, userClaim.ClaimValue)) == null)
                {
                    user.AddClaim(base.GuidGenerator, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
                }
            }

            var list = user.Claims.ToList<IdentityUserClaim>();
            foreach (var claim in list)
            {
                if (!input.Any((IdentityUserClaimDto c) => claim.ClaimType == c.ClaimType && claim.ClaimValue == c.ClaimValue))
                {
                    user.RemoveClaim(new Claim(claim.ClaimType, claim.ClaimValue));
                }
            }
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task LockAsync(Guid id, int seconds)
        {
            IdentityUser user = await this.UserManager.GetByIdAsync(id);

            var endDate = new DateTimeOffset(Clock.Now).AddSeconds(seconds);
            (await UserManager.SetLockoutEndDateAsync(user, endDate)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UnlockAsync(Guid id)
        {
            IdentityUser user = await this.UserManager.GetByIdAsync(id);
            await this.UserManager.SetLockoutEndDateAsync(user, null);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordInput input)
        {
            var user = await UserManager.GetByIdAsync(id);

            if (!input.NewPassword.IsNullOrEmpty())
            {
                (await UserManager.RemovePasswordAsync(user)).CheckErrors();
                (await UserManager.AddPasswordAsync(user, input.NewPassword)).CheckErrors();
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<bool> GetTwoFactorEnabledAsync(Guid id)
        {
            var user = await UserManager.GetByIdAsync(id);
            return await UserManager.GetTwoFactorEnabledAsync(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task ChangeTwoFactorEnabledAsync(Guid id, ChangeTwoFactorEnabledDto input)
        {
            var user = await UserManager.GetByIdAsync(id);

            if (input.Enabled)
            {
                var phoneNumberConfirmed = await UserManager.IsPhoneNumberConfirmedAsync(user);
                var emailAddressConfirmed = await UserManager.IsEmailConfirmedAsync(user);
                // 如果其中一个安全选项未确认,无法启用双因素验证
                if (!phoneNumberConfirmed && !emailAddressConfirmed)
                {
                    // TODO: 返回标准的 IdentityResult
                    //var error = new IdentityError();
                    //return IdentityResult.Failed(error);

                    throw new BusinessException(
                        IdentityErrorCodes.CanNotChangeTwoFactor,
                        details: phoneNumberConfirmed ? "phone number not confirmed" : "email address not confirmed");
                }
            }

            (await UserManager.SetTwoFactorEnabledAsync(user, input.Enabled)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            IdentityUser source = await this.UserManager.FindByNameAsync(username);
            IdentityUserDto result = ObjectMapper.Map<IdentityUser, IdentityUserDto>(source);
            return result;
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            IdentityUser source = await this.UserManager.FindByEmailAsync(email);
            IdentityUserDto result = ObjectMapper.Map<IdentityUser, IdentityUserDto>(source);
            return result;
        }

        protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

            (await UserManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled)).CheckErrors();
            (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;

            if (input.RoleNames != null)
            {
                (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }

            if (input.OrganizationUnitIds != null)
            {
                await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);
            }
        }
    }
}
