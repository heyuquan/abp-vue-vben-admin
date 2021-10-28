using System;
using System.Collections.Generic;
using System.Text;
using Leopard.BackendAdmin.Localization;
using Volo.Abp.Application.Services;

namespace Leopard.BackendAdmin
{
    /* Inherit your application services from this class.
     */
    public abstract class BackendAdminAppService : ApplicationService
    {
        protected BackendAdminAppService()
        {
            LocalizationResource = typeof(BackendAdminResource);
        }
    }
}
