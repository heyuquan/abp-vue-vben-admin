using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Microsoft.Extensions.DependencyInjection;
using Leopard.AuthServer;

namespace Leopard.AspNetCore.Swashbuckle
{
    [DependsOn(       
       typeof(AbpSwashbuckleModule),

       typeof(LeopardModule)
   )]
    public class LeopardAspNetCoreSwashbuckleModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AuthServerOptions>(configuration.GetSection("SwaggerClient"));
        }
    }
}
