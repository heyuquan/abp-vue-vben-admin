using Mk.DemoB.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mk.DemoB.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class DemoBController : AbpController
    {
        protected DemoBController()
        {
            LocalizationResource = typeof(DemoBResource);
        }
    }
}