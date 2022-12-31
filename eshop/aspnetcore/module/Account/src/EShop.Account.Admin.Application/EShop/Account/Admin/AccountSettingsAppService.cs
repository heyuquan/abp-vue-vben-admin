using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using EShop.Account.Settings;

namespace EShop.Account.Admin
{
    [Authorize(AccountPermissions.SettingManagement)]
	public class AccountSettingsAppService : ApplicationService, IRemoteService, IApplicationService, IAccountSettingsAppService
	{
		protected ISettingManager SettingManager { get; }

        public AccountSettingsAppService(ISettingManager settingManager)
        {
            this.SettingManager = settingManager;
        }

		public virtual async Task<AccountSettingsDto> GetAsync()
		{
			var accountSettingsDto = new AccountSettingsDto();
			bool isSelfRegistrationEnabled = await SettingProvider.GetAsync<bool>(AccountSettingNames.IsSelfRegistrationEnabled, false);
			accountSettingsDto.IsSelfRegistrationEnabled = isSelfRegistrationEnabled;
			bool enableLocalLogin = await SettingProvider.GetAsync<bool>(AccountSettingNames.EnableLocalLogin, false);
			accountSettingsDto.EnableLocalLogin = enableLocalLogin;
			bool isRememberBrowserEnabled = await SettingProvider.GetAsync<bool>(AccountSettingNames.TwoFactorLogin.IsRememberBrowserEnabled, false);
			accountSettingsDto.IsRememberBrowserEnabled = isRememberBrowserEnabled;
			return accountSettingsDto;
		}

		public virtual async Task UpdateAsync(AccountSettingsDto input)
		{
			if (input != null)
			{
				await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.IsSelfRegistrationEnabled, input.IsSelfRegistrationEnabled.ToString());
				await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.EnableLocalLogin, input.EnableLocalLogin.ToString());
				await SettingManager.SetForCurrentTenantAsync(AccountSettingNames.TwoFactorLogin.IsRememberBrowserEnabled, input.IsRememberBrowserEnabled.ToString());
			}
		}
	}
}
