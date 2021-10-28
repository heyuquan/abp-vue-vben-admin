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
        typeof(BackendAdminDomainModule),
        typeof(BackendAdminApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(SaasApplicationModule),
        typeof(LeopardAccountAdminApplicationModule),
        typeof(LeopardIdentityApplicationModule)
        )]
    public class BackendAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BackendAdminApplicationModule>();
            });
        }
    }
}
