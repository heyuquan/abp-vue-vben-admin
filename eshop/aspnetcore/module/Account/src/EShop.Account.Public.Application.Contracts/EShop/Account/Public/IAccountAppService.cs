using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Leopard.Identity;

namespace EShop.Account.Public
{
	/// <summary>
	/// 账户服务
	/// </summary>
    public interface IAccountAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 注册账户
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<IdentityUserDto> RegisterAsync(RegisterDto input);

		/// <summary>
		/// 发送密码重置码
		/// </summary>
		/// <returns></returns>
		Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input);
		/// <summary>
		/// 密码重置
		/// </summary>
		/// <returns></returns>
		Task ResetPasswordAsync(ResetPasswordDto input);

		/// <summary>
		/// 发送手机验证码
		/// </summary>
		/// <returns></returns>
		Task SendPhoneNumberConfirmationTokenAsync();
		/// <summary>
		/// 确认手机号码
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input);
		/// <summary>
		/// 确认邮箱
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task ConfirmEmailAsync(ConfirmEmailInput input);
	}
}
