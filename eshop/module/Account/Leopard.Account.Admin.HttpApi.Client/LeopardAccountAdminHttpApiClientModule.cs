using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Account.Admin
{
    [DependsOn(
        typeof(LeopardAccountAdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class LeopardAccountAdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AccountAdmin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(LeopardAccountAdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
