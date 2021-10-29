using Leopard.Account.Admin;
using Leopard.Identity;
using Leopard.Saas;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(LeopardBackendAdminDomainModule),
        typeof(LeopardBackendAdminApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(LeopardSaasApplicationModule),
        typeof(LeopardAccountAdminApplicationModule),
        typeof(LeopardIdentityApplicationModule)
        )]
    public class LeopardBackendAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LeopardBackendAdminApplicationModule>();
            });
        }
    }
}
