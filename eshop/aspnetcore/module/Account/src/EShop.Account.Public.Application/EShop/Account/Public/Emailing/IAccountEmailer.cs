using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace EShop.Account.Public.Emailing
{
	public interface IAccountEmailer
	{
		Task SendPasswordResetLinkAsync(IdentityUser user, string resetToken, string appName);

		Task SendEmailConfirmationLinkAsync(IdentityUser user, string confirmationToken, string appName);
	}
}
