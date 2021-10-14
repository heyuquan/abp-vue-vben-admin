using SSO.AuthServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SSO.AuthServer.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AuthServerController : AbpController
    {
        protected AuthServerController()
        {
            LocalizationResource = typeof(AuthServerResource);
        }
    }
}