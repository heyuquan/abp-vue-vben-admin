using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识设置服务
	/// </summary>
	public interface IIdentitySettingsAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 获取设置
		/// </summary>
		/// <returns></returns>
		Task<IdentitySettingsDto> GetAsync();

		/// <summary>
		/// 更新设置
		/// </summary>
		/// <returns></returns>
		Task UpdateAsync(IdentitySettingsDto input);
	}
}
