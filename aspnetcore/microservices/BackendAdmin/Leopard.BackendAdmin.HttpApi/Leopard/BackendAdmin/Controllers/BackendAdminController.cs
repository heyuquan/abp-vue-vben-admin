using Leopard.BackendAdmin.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.BackendAdmin.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BackendAdminController : AbpController
    {
        protected BackendAdminController()
        {
            LocalizationResource = typeof(BackendAdminResource);
        }
    }
}