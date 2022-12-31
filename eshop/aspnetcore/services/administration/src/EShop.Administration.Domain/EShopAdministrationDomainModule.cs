using Leopard.Saas;
using Volo.Abp.AuditLogging;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpEmailingModule),
        typeof(LeopardSaasDomainModule)
    )]
    public class EShopAdministrationDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
