using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Leopard.Identity
{
	[Authorize(IdentityPermissions.SettingManagement)]
	public class IdentitySettingsAppService : IdentityAppServiceBase, IIdentitySettingsAppService, IApplicationService, IRemoteService
	{
		protected ISettingManager SettingManager { get; }

		protected IdentityOptions IdentityOptions { get; }

		public IdentitySettingsAppService(ISettingManager settingManager, IOptions<IdentityOptions> identityOptions)
		{
			SettingManager = settingManager;
			IdentityOptions = identityOptions.Value;
		}

		public virtual async Task<IdentitySettingsDto> GetAsync()
		{
			var identitySettingsDto = new IdentitySettingsDto();
			identitySettingsDto.Password = new IdentityPasswordSettingsDto
			{
				RequiredLength = this.IdentityOptions.Password.RequiredLength,
				RequiredUniqueChars = this.IdentityOptions.Password.RequiredUniqueChars,
				RequireNonAlphanumeric = this.IdentityOptions.Password.RequireNonAlphanumeric,
				RequireLowercase = this.IdentityOptions.Password.RequireLowercase,
				RequireUppercase = this.IdentityOptions.Password.RequireUppercase,
				RequireDigit = this.IdentityOptions.Password.RequireDigit
			};
			identitySettingsDto.Lockout = new IdentityLockoutSettingsDto
			{
				AllowedForNewUsers = this.IdentityOptions.Lockout.AllowedForNewUsers,
				LockoutDuration = (int)this.IdentityOptions.Lockout.DefaultLockoutTimeSpan.TotalSeconds,
				MaxFailedAccessAttempts = this.IdentityOptions.Lockout.MaxFailedAccessAttempts
			};

			var identitySignInSettingsDto = new IdentitySignInSettingsDto();
			identitySignInSettingsDto.RequireConfirmedEmail = this.IdentityOptions.SignIn.RequireConfirmedEmail;
			bool enablePhoneNumberConfirmation = await base.SettingProvider.GetAsync("Abp.Identity.SignIn.EnablePhoneNumberConfirmation", true);
			identitySignInSettingsDto.EnablePhoneNumberConfirmation = enablePhoneNumberConfirmation;
			identitySignInSettingsDto.RequireConfirmedPhoneNumber = this.IdentityOptions.SignIn.RequireConfirmedPhoneNumber;

			identitySettingsDto.SignIn = identitySignInSettingsDto;

			var identityUserSettingsDto = new IdentityUserSettingsDto();
			bool isEmailUpdateEnabled = await base.SettingProvider.GetAsync("Abp.Identity.User.IsEmailUpdateEnabled", true);
			identityUserSettingsDto.IsEmailUpdateEnabled = isEmailUpdateEnabled;
			bool isUserNameUpdateEnabled = await base.SettingProvider.GetAsync("Abp.Identity.User.IsUserNameUpdateEnabled", true);
			identityUserSettingsDto.IsUserNameUpdateEnabled = isUserNameUpdateEnabled;

			identitySettingsDto.User = identityUserSettingsDto;

			return identitySettingsDto;
		}

		public virtual async Task UpdateAsync(IdentitySettingsDto input)
		{
			if (input.Password != null)
			{
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequiredLength", input.Password.RequiredLength.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequiredUniqueChars", input.Password.RequiredUniqueChars.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequireNonAlphanumeric", input.Password.RequireNonAlphanumeric.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequireLowercase", input.Password.RequireLowercase.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequireUppercase", input.Password.RequireUppercase.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Password.RequireDigit", input.Password.RequireDigit.ToString());
			}
			if (input.Lockout != null)
			{
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Lockout.AllowedForNewUsers", input.Lockout.AllowedForNewUsers.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Lockout.MaxFailedAccessAttempts", input.Lockout.MaxFailedAccessAttempts.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.Lockout.LockoutDuration", input.Lockout.LockoutDuration.ToString());
			}
			if (input.SignIn != null)
			{
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.SignIn.RequireConfirmedEmail", input.SignIn.RequireConfirmedEmail.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.SignIn.RequireConfirmedPhoneNumber", input.SignIn.RequireConfirmedPhoneNumber.ToString());
				bool flag = input.SignIn.EnablePhoneNumberConfirmation || input.SignIn.RequireConfirmedPhoneNumber;
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.SignIn.EnablePhoneNumberConfirmation", flag.ToString());
			}
			if (input.User != null)
			{
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.User.IsUserNameUpdateEnabled", input.User.IsUserNameUpdateEnabled.ToString());
				await this.SettingManager.SetForCurrentTenantAsync("Abp.Identity.User.IsEmailUpdateEnabled", input.User.IsEmailUpdateEnabled.ToString());
			}
		}
	}
}
