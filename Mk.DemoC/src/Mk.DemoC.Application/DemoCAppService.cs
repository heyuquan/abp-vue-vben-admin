using Mk.DemoC.Localization;
using Volo.Abp.Application.Services;

namespace Mk.DemoC
{
    public abstract class DemoCAppService : ApplicationService
    {
        protected DemoCAppService()
        {
            LocalizationResource = typeof(DemoCResource);
            ObjectMapperContext = typeof(DemoCApplicationModule);
        }
    }
}
