using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Leopard.Identity
{
	/// <summary>
	/// 查询用户服务
	/// </summary>
	public interface IIdentityUserLookupAppService : IApplicationService, IRemoteService
	{
		/// <summary>
		/// 根据Id查找用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<UserData> FindByIdAsync(Guid id);
		/// <summary>
		/// 根据用户名查找用户
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		Task<UserData> FindByUserNameAsync(string userName);
		/// <summary>
		/// 查找用户
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input);
		/// <summary>
		/// 查找匹配条件的用户数量
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<long> GetCountAsync(UserLookupCountInputDto input);
	}
}
