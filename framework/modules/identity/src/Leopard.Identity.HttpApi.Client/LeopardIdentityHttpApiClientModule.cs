using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Identity
{
    [DependsOn(
        typeof(LeopardIdentityApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class LeopardIdentityHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Identity";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(LeopardIdentityApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
