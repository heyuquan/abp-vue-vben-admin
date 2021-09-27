using Leopard.AuthServer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Leopard
{
    public class LeopardModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AuthServerOptions>(configuration.GetSection(AuthServerOptions.SectionName));
        }
    }
}
