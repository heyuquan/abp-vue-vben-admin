using Mk.DemoC.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mk.DemoC
{
    public abstract class DemoCController : AbpController
    {
        protected DemoCController()
        {
            LocalizationResource = typeof(DemoCResource);
        }
    }
}
