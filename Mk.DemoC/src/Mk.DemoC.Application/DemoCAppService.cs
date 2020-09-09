using Mk.DemoC.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Tracing;

namespace Mk.DemoC
{
    public abstract class DemoCAppService : ApplicationService
    {
        protected DemoCAppService()
        {
            LocalizationResource = typeof(DemoCResource);
            ObjectMapperContext = typeof(DemoCApplicationModule);
        }

        private ICorrelationIdProvider _correlationIdProvider;

        protected ICorrelationIdProvider IdProvider => LazyGetRequiredService(ref _correlationIdProvider);
    }
}
