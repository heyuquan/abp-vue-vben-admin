using System;
using System.Collections.Generic;
using System.Text;
using EShop.Administration.Localization;
using Volo.Abp.Application.Services;

namespace EShop.Administration
{
    /* Inherit your application services from this class.
     */
    public abstract class AdministrationAppService : ApplicationService
    {
        protected AdministrationAppService()
        {
            LocalizationResource = typeof(AdministrationResource);
        }
    }
}
