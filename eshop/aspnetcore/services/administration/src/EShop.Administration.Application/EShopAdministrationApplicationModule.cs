using EShop.Account.Admin;
using Leopard.Identity;
using Leopard.Saas;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainModule),
        typeof(AdministrationApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(LeopardSaasApplicationModule),
        typeof(EShopAccountAdminApplicationModule),
        typeof(LeopardIdentityApplicationModule)
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
