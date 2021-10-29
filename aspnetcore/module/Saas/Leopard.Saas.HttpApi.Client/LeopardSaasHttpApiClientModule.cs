using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(LeopardSaasApplicationContractsModule)
        )]
    public class LeopardSaasHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(LeopardSaasApplicationContractsModule).Assembly,
                SaasRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}