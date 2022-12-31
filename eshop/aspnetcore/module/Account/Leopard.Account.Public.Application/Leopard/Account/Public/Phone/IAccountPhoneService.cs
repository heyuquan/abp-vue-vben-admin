using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Leopard.Account.Public.Phone
{
	public interface IAccountPhoneService
	{
		Task SendConfirmationCodeAsync(IdentityUser user, string confirmationToken);
	}
}
