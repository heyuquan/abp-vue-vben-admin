using EShop.Account.Admin;
using Leopard.Identity;
using Leopard.Saas;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainSharedModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule),
        typeof(LeopardSaasApplicationContractsModule),
        typeof(EShopAccountAdminApplicationContractsModule),
        typeof(LeopardIdentityApplicationContractsModule)
    )]
    public class AdministrationApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AdministrationDtoExtensions.Configure();
        }
    }
}
