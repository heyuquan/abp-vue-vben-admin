using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
	/// <summary>
	/// 个人信息服务
	/// </summary>
	public interface IProfileAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 获取用户基本信息
		/// </summary>
		/// <returns></returns>
		Task<ProfileDto> GetAsync();
		/// <summary>
		/// 更新用户基本信息
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<ProfileDto> UpdateAsync(UpdateProfileDto input);
		/// <summary>
		/// 更改密码
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task ChangePasswordAsync(ChangePasswordInput input);
	}
}
