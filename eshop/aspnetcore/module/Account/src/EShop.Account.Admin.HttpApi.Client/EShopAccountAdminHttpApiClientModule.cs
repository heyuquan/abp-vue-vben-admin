using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EShop.Account.Admin
{
    [DependsOn(
        typeof(EShopAccountAdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class EShopAccountAdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AccountAdmin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(EShopAccountAdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
