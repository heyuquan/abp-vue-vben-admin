using Leopard.Account.Admin;
using Leopard.BackendAdmin.Localization;
using Leopard.BackendAdmin.Web;
using Leopard.Identity;
using Leopard.Saas;
using Localization.Resources.AbpUi;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(LeopardBackendAdminApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(LeopardSaasHttpApiModule),
        typeof(LeopardAccountAdminHttpApiModule),
        typeof(LeopardIdentityHttpApiModule),
        typeof(LeopardBackendAdminWebModule)
        )]
    public class LeopardBackendAdminHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BackendAdminResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
