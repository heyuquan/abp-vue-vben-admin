using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace EShop.Account.Admin
{
	/// <summary>
	/// 账户设置
	/// </summary>
	[Route("api/account/admin/settings")]
	[RemoteService(true, Name = AccountAdminRemoteServiceConsts.RemoteServiceName)]
	public class AccountSettingsController : AbpController, IAccountSettingsAppService, IRemoteService, IApplicationService
	{
		protected IAccountSettingsAppService AccountSettingsAppService { get; }

		public AccountSettingsController(IAccountSettingsAppService accountSettingsAppService)
		{
			this.AccountSettingsAppService = accountSettingsAppService;
		}

		/// <summary>
		/// 获取账户设置
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public virtual async Task<AccountSettingsDto> GetAsync()
		{
			return await this.AccountSettingsAppService.GetAsync();
		}

		/// <summary>
		/// 更新账户设置
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		public virtual async Task UpdateAsync(AccountSettingsDto input)
		{
			await this.AccountSettingsAppService.UpdateAsync(input);
		}
	}
}
