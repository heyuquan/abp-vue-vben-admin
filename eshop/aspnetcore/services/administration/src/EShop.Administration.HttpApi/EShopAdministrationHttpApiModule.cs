using EShop.Account.Admin;
using EShop.Administration.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(EShopAdministrationApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(EShopAccountAdminHttpApiModule)
        )]
    public class EShopAdministrationHttpApiModule : AbpModule
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
                    .Get<AdministrationResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
