using Leopard.AspNetCore.Swashbuckle.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

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
            context.Services.Configure<SwaggerOptions>(configuration.GetSection(SwaggerOptions.SectionName));
        }
    }
}
