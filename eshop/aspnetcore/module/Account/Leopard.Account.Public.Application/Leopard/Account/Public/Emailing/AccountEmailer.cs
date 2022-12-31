using Microsoft.Extensions.Localization;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;
using Leopard.Account.Localization;

namespace Leopard.Account.Public.Emailing
{
    public class AccountEmailer : IAccountEmailer, ITransientDependency
	{
		protected ITemplateRenderer TemplateRenderer { get; }

		protected IEmailSender EmailSender { get; }

		protected IStringLocalizer<LeopardAccountResource> StringLocalizer { get; }

		protected IAppUrlProvider AppUrlProvider { get; }

		protected ICurrentTenant CurrentTenant { get; }

		public AccountEmailer(IEmailSender emailSender, ITemplateRenderer templateRenderer, IStringLocalizer<LeopardAccountResource> stringLocalizer, IAppUrlProvider appUrlProvider, ICurrentTenant currentTenant)
		{
			EmailSender = emailSender;
			StringLocalizer = stringLocalizer;
			AppUrlProvider = appUrlProvider;
			CurrentTenant = currentTenant;
			TemplateRenderer = templateRenderer;
		}

		public virtual async Task SendPasswordResetLinkAsync(IdentityUser user, string resetToken, string appName)
		{
			string text = await this.AppUrlProvider.GetResetPasswordUrlAsync(appName);
			string body = await this.TemplateRenderer.RenderAsync("Abp.Account.PasswordResetLink", string.Format("{0}?userId={1}&tenantId={2}&resetToken={3}", new object[]
			{
				text,
				user.Id,
				user.TenantId,
				UrlEncoder.Default.Encode(resetToken)
			}), null, null);
			await this.EmailSender.SendAsync(user.Email, this.StringLocalizer["PasswordReset"], body, true);
		}

		public async Task SendEmailConfirmationLinkAsync(IdentityUser user, string confirmationToken, string appName)
		{
			string text = await this.AppUrlProvider.GetEmailConfirmationUrlAsync(appName);
			string body = await this.TemplateRenderer.RenderAsync("Abp.Account.EmailConfirmationLink", string.Format("{0}?userId={1}&tenantId={2}&confirmationToken={3}", new object[]
			{
				text,
				user.Id,
				user.TenantId,
				UrlEncoder.Default.Encode(confirmationToken)
			}), null, null);
			await this.EmailSender.SendAsync(user.Email, this.StringLocalizer["EmailConfirmation"], body, true);
		}
	}
}
