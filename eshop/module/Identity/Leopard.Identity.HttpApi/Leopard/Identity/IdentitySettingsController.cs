using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识设置服务
	/// </summary>
	[Route("api/identity/settings")]
	[ControllerName("Settings")]
	[RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
	[Area("identity")]
	public class IdentitySettingsController : AbpController, IIdentitySettingsAppService, IApplicationService, IRemoteService
	{
		protected IIdentitySettingsAppService IdentitySettingsAppService { get; }

		public IdentitySettingsController(IIdentitySettingsAppService identitySettingsAppService)
		{
			IdentitySettingsAppService = identitySettingsAppService;
		}

		/// <summary>
		/// 获取设置
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public virtual Task<IdentitySettingsDto> GetAsync()
		{
			return this.IdentitySettingsAppService.GetAsync();
		}

		/// <summary>
		/// 更新设置
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPut]
		public virtual Task UpdateAsync(IdentitySettingsDto input)
		{
			return this.IdentitySettingsAppService.UpdateAsync(input);
		}
	}
}
