using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Localization;

namespace EShop.Identity;

/* Inherit your application services from this class.
 */
public abstract class IdentityAppService : ApplicationService
{
    protected IdentityAppService()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
