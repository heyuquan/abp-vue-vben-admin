using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Localization;

namespace Leopard.Identity
{
	public abstract class IdentityAppServiceBase : ApplicationService
	{
		protected IdentityAppServiceBase()
		{
			ObjectMapperContext = typeof(LeopardIdentityApplicationModule);
			LocalizationResource = typeof(IdentityResource);
		}
	}
}
