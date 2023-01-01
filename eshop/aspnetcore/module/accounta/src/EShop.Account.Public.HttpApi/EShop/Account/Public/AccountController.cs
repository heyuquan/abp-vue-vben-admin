using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Leopard.Identity;

namespace EShop.Account.Public
{
	/// <summary>
	/// 账户服务
	/// </summary>
	[RemoteService(true, Name = AccountPublicRemoteServiceConsts.RemoteServiceName)]
	[Route("api/account/public/")]
	public class AccountController : AbpController, IAccountAppService, IRemoteService, IApplicationService
	{
		protected IAccountAppService AccountAppService { get; }

		public AccountController(IAccountAppService accountAppService)
		{
			this.AccountAppService = accountAppService;
		}

		/// <summary>
		/// 注册账户
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("register")]
		[HttpPost]
		public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
		{
			return this.AccountAppService.RegisterAsync(input);
		}

		/// <summary>
		/// 发送密码重置码
		/// </summary>
		/// <returns></returns>
		[Route("send-password-reset-code")]
		[HttpPost]
		public virtual Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
		{
			return this.AccountAppService.SendPasswordResetCodeAsync(input);
		}

		/// <summary>
		/// 密码重置
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("reset-password")]
		public virtual Task ResetPasswordAsync(ResetPasswordDto input)
		{
			return this.AccountAppService.ResetPasswordAsync(input);
		}

		/// <summary>
		/// 发送手机验证码
		/// </summary>
		/// <returns></returns>
		[Route("send-phone-number-confirmation-token")]
		[HttpPost]
		public Task SendPhoneNumberConfirmationTokenAsync()
		{
			return this.AccountAppService.SendPhoneNumberConfirmationTokenAsync();
		}

		/// <summary>
		/// 确认手机号码
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("confirm-phone-number")]
		public Task ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
		{
			return this.AccountAppService.ConfirmPhoneNumberAsync(input);
		}

		/// <summary>
		/// 确认邮箱
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("confirm-email")]
		[HttpPost]
		public Task ConfirmEmailAsync(ConfirmEmailInput input)
		{
			return this.AccountAppService.ConfirmEmailAsync(input);
		}
	}
}
