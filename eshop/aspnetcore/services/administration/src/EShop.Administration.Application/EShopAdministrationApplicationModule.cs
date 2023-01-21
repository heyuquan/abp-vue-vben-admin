using EShop.Account.Admin;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainModule),
        typeof(EShopAdministrationApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(EShopAccountAdminApplicationModule)
        )]
    public class EShopAdministrationApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EShopAdministrationApplicationModule>();
            });
        }
    }
}
