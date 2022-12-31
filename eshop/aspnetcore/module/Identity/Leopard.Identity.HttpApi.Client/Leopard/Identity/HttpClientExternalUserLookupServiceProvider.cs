using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using DependencyAttribute = Volo.Abp.DependencyInjection.DependencyAttribute;

namespace Leopard.Identity
{
    [Dependency(TryRegister = true)]
	public class HttpClientExternalUserLookupServiceProvider : ITransientDependency, IExternalUserLookupServiceProvider
	{
		protected IIdentityUserLookupAppService UserLookupAppService { get; }

		public HttpClientExternalUserLookupServiceProvider(IIdentityUserLookupAppService userLookupAppService)
		{
			this.UserLookupAppService = userLookupAppService;
		}

		public virtual async Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await this.UserLookupAppService.FindByIdAsync(id);
		}

		public virtual async Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await this.UserLookupAppService.FindByUserNameAsync(userName);
		}

		public async Task<List<IUserData>> SearchAsync(string sorting = null, string filter = null, int maxResultCount = 2147483647, int skipCount = 0, CancellationToken cancellationToken = default(CancellationToken))
		{
			return (await this.UserLookupAppService.SearchAsync(new UserLookupSearchInputDto
			{
				Sorting = sorting,
				Filter = filter,
				MaxResultCount = maxResultCount,
				SkipCount = skipCount
			})).Items.Cast<IUserData>().ToList<IUserData>();
		}

		public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await this.UserLookupAppService.GetCountAsync(new UserLookupCountInputDto
			{
				Filter = filter
			});
		}
	}
}
