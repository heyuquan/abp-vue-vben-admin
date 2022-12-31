using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.Identity
{
	/// <summary>
	/// 个人信息服务
	/// </summary>
	[Route("/api/identity/my-profile")]
	[ControllerName("Profile")]
	[RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
	[Area("identity")]
	public class ProfileController : AbpController, IProfileAppService, IApplicationService, IRemoteService
	{
		protected IProfileAppService ProfileAppService { get; }

		public ProfileController(IProfileAppService profileAppService)
		{
			ProfileAppService = profileAppService;
		}

		/// <summary>
		/// 获取用户基本信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public virtual Task<ProfileDto> GetAsync()
		{
			return this.ProfileAppService.GetAsync();
		}

		/// <summary>
		/// 更新用户基本信息
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[HttpPut]
		public virtual Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
		{
			return this.ProfileAppService.UpdateAsync(input);
		}

		/// <summary>
		/// 更改密码
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("change-password")]
		[HttpPost]
		public virtual Task ChangePasswordAsync(ChangePasswordInput input)
		{
			return this.ProfileAppService.ChangePasswordAsync(input);
		}
	}
}
