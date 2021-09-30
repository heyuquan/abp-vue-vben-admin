using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(SaasApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SaasHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Saas";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SaasApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
