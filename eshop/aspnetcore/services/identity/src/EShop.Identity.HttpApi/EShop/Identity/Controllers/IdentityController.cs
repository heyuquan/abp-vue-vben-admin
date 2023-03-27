using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.Localization;

namespace EShop.Identity.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class IdentityController : AbpControllerBase
{
    protected IdentityController()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
