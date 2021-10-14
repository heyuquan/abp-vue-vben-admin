using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Leopard.Account.Public
{
    [DependsOn(
        typeof(LeopardAccountPublicApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class YunAccountPublicHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AccountPublic";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(LeopardAccountPublicApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
