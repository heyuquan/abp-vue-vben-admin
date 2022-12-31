using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EShop.Account.Public
{
    [DependsOn(
        typeof(EShopAccountPublicApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class EShopAccountPublicHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AccountPublic";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(EShopAccountPublicApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
