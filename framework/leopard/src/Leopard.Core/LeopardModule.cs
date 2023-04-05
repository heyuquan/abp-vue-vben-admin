using Leopard.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Leopard
{
    public class LeopardModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddOptions<ApplicationOptions>()
                            .Bind(configuration.GetSection(ApplicationOptions.SectionName))
                            .ValidateDataAnnotations()
                            .ValidateOnStart();
            Configure<AuthOptions>(configuration.GetSection(AuthOptions.SectionName));
        }
    }
}
