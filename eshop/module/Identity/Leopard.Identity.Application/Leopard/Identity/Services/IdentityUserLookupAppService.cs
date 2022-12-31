using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Leopard.Identity
{
	[Authorize(IdentityPermissions.UserLookup.Default)]
	public class IdentityUserLookupAppService : IdentityAppServiceBase, IIdentityUserLookupAppService, IApplicationService, IRemoteService
	{
		protected IdentityUserRepositoryExternalUserLookupServiceProvider UserLookupServiceProvider { get; }

		public IdentityUserLookupAppService(IdentityUserRepositoryExternalUserLookupServiceProvider userLookupServiceProvider)
		{
			UserLookupServiceProvider = userLookupServiceProvider;
		}

		public virtual async Task<UserData> FindByIdAsync(Guid id)
		{
			IUserData userData = await this.UserLookupServiceProvider.FindByIdAsync(id);
			UserData result;
			if (userData == null)
			{
				result = null;
			}
			else
			{
				result = new UserData(userData);
			}
			return result;
		}

		public virtual async Task<UserData> FindByUserNameAsync(string userName)
		{
			IUserData userData = await this.UserLookupServiceProvider.FindByUserNameAsync(userName);
			UserData result;
			if (userData == null)
			{
				result = null;
			}
			else
			{
				result = new UserData(userData);
			}
			return result;
		}

		public async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
		{
			return new ListResultDto<UserData>((from u in await this.UserLookupServiceProvider.SearchAsync(input.Sorting, input.Filter, input.MaxResultCount, input.SkipCount)
			select new UserData(u)).ToList<UserData>());
		}

		public async Task<long> GetCountAsync(UserLookupCountInputDto input)
		{
			return await this.UserLookupServiceProvider.GetCountAsync(input.Filter);
		}
	}
}
