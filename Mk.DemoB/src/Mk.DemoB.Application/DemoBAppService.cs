using System;
using System.Collections.Generic;
using System.Text;
using Mk.DemoB.Localization;
using Volo.Abp.Application.Services;

namespace Mk.DemoB
{
    /* Inherit your application services from this class.
     */
    public abstract class DemoBAppService : ApplicationService
    {
        protected DemoBAppService()
        {
            LocalizationResource = typeof(DemoBResource);
        }
    }
}
