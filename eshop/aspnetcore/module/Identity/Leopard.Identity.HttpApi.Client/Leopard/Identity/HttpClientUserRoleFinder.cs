using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using DependencyAttribute = Volo.Abp.DependencyInjection.DependencyAttribute;

namespace Leopard.Identity
{
    [Dependency(TryRegister = true)]
	public class HttpClientUserRoleFinder : ITransientDependency, IUserRoleFinder
	{
		protected IIdentityUserAppService _userAppService { get; }

		public HttpClientUserRoleFinder(IIdentityUserAppService userAppService)
		{
			this._userAppService = userAppService;
		}

		public virtual async Task<string[]> GetRolesAsync(Guid userId)
		{
			return (await this._userAppService.GetRolesAsync(userId)).Items.Select(x => x.Name).ToArray<string>();
		}
	}
}
