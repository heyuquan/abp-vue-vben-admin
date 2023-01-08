using System;
using System.Collections.Generic;
using System.Text;
using EShop.Identity.Localization;
using Volo.Abp.Application.Services;

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
