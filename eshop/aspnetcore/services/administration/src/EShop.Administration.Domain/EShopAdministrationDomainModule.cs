using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule)
    )]
    public class EShopAdministrationDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
