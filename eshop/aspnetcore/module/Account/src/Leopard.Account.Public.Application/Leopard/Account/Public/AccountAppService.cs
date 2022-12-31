using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using Leopard.Account.Public.Emailing;
using Leopard.Account.Localization;
using Leopard.Account.Public.Phone;
using Leopard.Account.Settings;
using Leopard.Identity;

namespace Leopard.Account.Public
{
    public class AccountAppService : ApplicationService, IAccountAppService, IApplicationService, IRemoteService
	{
		protected IIdentityRoleRepository RoleRepository { get; }

		protected IdentityUserManager UserManager { get; }

		protected IAccountEmailer AccountEmailer { get; }

		public IAccountPhoneService PhoneService { get; }

		public AccountAppService(IdentityUserManager userManager, IAccountEmailer accountEmailer, IAccountPhoneService phoneService, IIdentityRoleRepository roleRepository)
		{
			RoleRepository = roleRepository;
			UserManager = userManager;
			AccountEmailer = accountEmailer;
			PhoneService = phoneService;
			base.LocalizationResource = typeof(LeopardAccountResource);
		}

		public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
		{
			await CheckSelfRegistrationAsync();

			var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

			(await UserManager.CreateAsync(user, input.Password)).CheckErrors();

			await UserManager.SetEmailAsync(user, input.EmailAddress);
			await UserManager.AddDefaultRolesAsync(user);

			return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
		}

		public virtual async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
		{
			var user = await this.GetUserByEmail(input.Email);
			string resetToken = await this.UserManager.GeneratePasswordResetTokenAsync(user);
			await this.AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName);
		}

		public virtual async Task ResetPasswordAsync(ResetPasswordDto input)
		{
			IdentityUser user = await this.UserManager.GetByIdAsync(input.UserId);
			(await this.UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();
		}

		public virtual async Task SendPhoneNumberConfirmationTokenAsync()
		{
			await this.CheckIfPhoneNumberConfirmationEnabledAsync();
			var user = await this.UserManager.GetByIdAsync(CurrentUser.GetId());
			this.CheckPhoneNumber(user);
			string confirmationToken = await this.UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
			await this.PhoneService.SendConfirmationCodeAsync(user, confirmationToken);
		}

		public virtual async Task ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
		{
			await this.CheckIfPhoneNumberConfirmationEnabledAsync();
			var identityUser = await this.UserManager.GetByIdAsync(CurrentUser.GetId());
			this.CheckPhoneNumber(identityUser);
			(await this.UserManager.ChangePhoneNumberAsync(identityUser, identityUser.PhoneNumber, input.Token)).CheckErrors();
		}

		public virtual async Task ConfirmEmailAsync(ConfirmEmailInput input)
		{
			var identityUser = await this.UserManager.GetByIdAsync(CurrentUser.GetId());
			(await this.UserManager.ChangeEmailAsync(identityUser, identityUser.Email, input.Token)).CheckErrors();
		}

		protected virtual async Task<IdentityUser> GetUserByEmail(string email)
		{
			var user = await this.UserManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new BusinessException("Leopard.Account:InvalidEmailAddress").WithData("Email", email);
			}
			return user;
		}

		protected virtual async Task CheckSelfRegistrationAsync()
		{
			var flag = await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled);

			if (!flag)
			{
				throw new BusinessException("Leopard.Account:SelfRegistrationDisabled");
			}
		}

		protected virtual void CheckPhoneNumber(IdentityUser user)
		{
			if (string.IsNullOrEmpty(user.PhoneNumber))
			{
				throw new BusinessException("Leopard.Account:PhoneNumberEmpty");
			}
		}

		protected virtual async Task CheckIfPhoneNumberConfirmationEnabledAsync()
		{
			bool flag = await SettingProvider.IsTrueAsync("Abp.Identity.SignIn.EnablePhoneNumberConfirmation");

			if (!flag)
			{
				throw new BusinessException("Leopard.Account:PhoneNumberConfirmationDisabled");
			}
		}
	}
}
