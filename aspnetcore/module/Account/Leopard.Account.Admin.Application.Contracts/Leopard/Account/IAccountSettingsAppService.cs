using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Leopard.Account
{
	/// <summary>
	/// 账户设置
	/// </summary>
	public interface IAccountSettingsAppService : IRemoteService, IApplicationService
	{
		/// <summary>
		/// 获取账户设置
		/// </summary>
		/// <returns></returns>
		Task<AccountSettingsDto> GetAsync();

		/// <summary>
		/// 更新账户设置
		/// </summary>
		/// <returns></returns>
		Task UpdateAsync(AccountSettingsDto input);
	}
}
