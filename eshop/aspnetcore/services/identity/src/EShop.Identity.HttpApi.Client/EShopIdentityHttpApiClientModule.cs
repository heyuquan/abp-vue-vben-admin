using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShop.Identity;

[DependsOn(
    typeof(EShopIdentityApplicationContractsModule),
    typeof(AbpIdentityHttpApiClientModule)
)]
public class EShopIdentityHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(EShopIdentityApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EShopIdentityHttpApiClientModule>();
        });
    }
}
