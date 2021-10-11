using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(SaasApplicationContractsModule)
        )]
    public class SaasServiceHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SaasApplicationContractsModule).Assembly,
                SaasRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}