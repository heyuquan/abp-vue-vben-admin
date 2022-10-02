using System;
using System.Collections.Generic;
using System.Text;
using Mk.DemoB.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Tracing;

namespace Mk.DemoB
{
    /* Inherit your application services from this class.
     */
    public abstract class DemoBAppService : ApplicationService
    {

        protected DemoBAppService()
        {
            LocalizationResource = typeof(DemoBResource);
        }

        protected ICorrelationIdProvider CorrelationIdIdProvider => LazyServiceProvider.LazyGetRequiredService<ICorrelationIdProvider>();
    }
}
