using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Leopard.Identity
{
	/// <summary>
	/// 查询用户服务
	/// </summary>
	[Area("identity")]
	[ControllerName("UserLookup")]
	[RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
	[Route("api/identity/users/lookup")]
	public class IdentityUserLookupController : AbpController, IIdentityUserLookupAppService, IApplicationService, IRemoteService
	{
		protected IIdentityUserLookupAppService LookupAppService { get; }

		public IdentityUserLookupController(IIdentityUserLookupAppService lookupAppService)
		{
			LookupAppService = lookupAppService;
		}

	    /// <summary>
		/// 根据Id查找用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}")]
		public virtual Task<UserData> FindByIdAsync(Guid id)
		{
			return this.LookupAppService.FindByIdAsync(id);
		}
		/// <summary>
		/// 根据用户名查找用户
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("by-username/{userName}")]
		public virtual Task<UserData> FindByUserNameAsync(string userName)
		{
			return this.LookupAppService.FindByUserNameAsync(userName);
		}
		/// <summary>
		/// 查找用户
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("search")]
		[HttpGet]
		public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
		{
			return this.LookupAppService.SearchAsync(input);
		}
		/// <summary>
		/// 查找匹配条件的用户数量
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[Route("count")]
		[HttpGet]
		public Task<long> GetCountAsync(UserLookupCountInputDto input)
		{
			return this.LookupAppService.GetCountAsync(input);
		}
	}
}
