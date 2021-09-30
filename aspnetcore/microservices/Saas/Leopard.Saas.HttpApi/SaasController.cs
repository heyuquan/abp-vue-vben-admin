using Leopard.Saas.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.Saas
{
    public abstract class SaasController : AbpController
    {
        protected SaasController()
        {
            LocalizationResource = typeof(SaasResource);
        }
    }
}
