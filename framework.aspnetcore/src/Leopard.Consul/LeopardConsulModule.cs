using Leopard.Consul.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard.Consul
{
    public class LeopardConsulModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddServiceDiscovery();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseConsulRegisterService();
        }
    }
}
