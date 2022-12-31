using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(DemoBApplicationContractsModule)
    )]
    public class DemoBHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "DemoB";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(DemoBApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
