using EShop.Administration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EShop.Administration.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BackendAdminController : AbpController
    {
        protected BackendAdminController()
        {
            LocalizationResource = typeof(AdministrationResource);
        }
    }
}