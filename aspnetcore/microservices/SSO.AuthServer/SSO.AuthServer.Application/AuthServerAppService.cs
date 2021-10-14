using System;
using System.Collections.Generic;
using System.Text;
using SSO.AuthServer.Localization;
using Volo.Abp.Application.Services;

namespace SSO.AuthServer
{
    /* Inherit your application services from this class.
     */
    public abstract class AuthServerAppService : ApplicationService
    {
        protected AuthServerAppService()
        {
            LocalizationResource = typeof(AuthServerResource);
        }
    }
}
