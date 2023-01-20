using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Sms;
using EShop.Account.Localization;

namespace EShop.Account.Public.Phone
{
    public class AccountPhoneService : IAccountPhoneService, ITransientDependency
	{
		public ISmsSender SmsSender { get; }

		private readonly IStringLocalizer<EShopAccountResource> Localizer;

		public AccountPhoneService(ISmsSender smsSender, IStringLocalizer<EShopAccountResource> localizer)
		{
			Localizer = localizer;
			SmsSender = smsSender;
		}

		public virtual async Task SendConfirmationCodeAsync(IdentityUser user, string confirmationToken)
		{
			string text;
			if (!string.IsNullOrWhiteSpace(user.Name))
			{
				string name = user.Name;
				string surname = user.Surname;
				text = name + ((surname != null) ? surname.EnsureStartsWith(' ', StringComparison.Ordinal) : null);
			}
			else
			{
				text = user.UserName;
			}
			await this.SmsSender.SendAsync(user.PhoneNumber, this.Localizer["PhoneConfirmationSms", new object[]
			{
				text,
				confirmationToken
			}]);
		}
	}
}
