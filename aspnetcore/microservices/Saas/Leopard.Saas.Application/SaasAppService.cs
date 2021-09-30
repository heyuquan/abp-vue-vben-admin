using Leopard.Saas.Localization;
using Volo.Abp.Application.Services;

namespace Leopard.Saas
{
    public abstract class SaasAppService : ApplicationService
    {
        protected SaasAppService()
        {
            LocalizationResource = typeof(SaasResource);
            ObjectMapperContext = typeof(SaasApplicationModule);
        }
    }
}
