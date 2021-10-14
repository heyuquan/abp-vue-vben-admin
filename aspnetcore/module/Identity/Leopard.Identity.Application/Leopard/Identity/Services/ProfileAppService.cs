using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Leopard.Identity
{
	[Authorize]
	public class ProfileAppService : IdentityAppServiceBase, IProfileAppService, IApplicationService, IRemoteService
	{
		protected IdentityUserManager UserManager { get; }

		public ProfileAppService(IdentityUserManager userManager)
		{
			UserManager = userManager;
		}

		public virtual async Task<ProfileDto> GetAsync()
		{
			var source = await this.UserManager.GetByIdAsync(base.CurrentUser.GetId());
			return ObjectMapper.Map<IdentityUser, ProfileDto>(source);
		}

		public virtual async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
		{
			var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

			if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
			{
				(await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
			}

			if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
			{
				(await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
			}

			(await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();

			user.Name = input.Name;
			user.Surname = input.Surname;

			input.MapExtraPropertiesTo(user);

			(await UserManager.UpdateAsync(user)).CheckErrors();

			await CurrentUnitOfWork.SaveChangesAsync();

			return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
		}

		public virtual async Task ChangePasswordAsync(ChangePasswordInput input)
		{
			var user = await this.UserManager.GetByIdAsync(base.CurrentUser.GetId());
			(await this.UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword)).CheckErrors();
		}
	}
}
