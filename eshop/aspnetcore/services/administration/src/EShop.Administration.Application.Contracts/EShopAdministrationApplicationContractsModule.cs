using EShop.Account.Admin;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainSharedModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(EShopAccountAdminApplicationContractsModule)
    )]
    public class EShopAdministrationApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AdministrationDtoExtensions.Configure();
        }
    }
}
