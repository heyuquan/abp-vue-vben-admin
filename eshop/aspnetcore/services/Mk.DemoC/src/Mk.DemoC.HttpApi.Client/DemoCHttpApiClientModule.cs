using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class DemoCHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "DemoC";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(DemoCApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
