using Leopard.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace EShop.Identity;

[DependsOn(
    typeof(EShopIdentityDomainModule),
    typeof(EShopIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(LeopardIdentityApplicationModule)
    )]
public class EShopIdentityApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EShopIdentityApplicationModule>();
        });
    }
}
