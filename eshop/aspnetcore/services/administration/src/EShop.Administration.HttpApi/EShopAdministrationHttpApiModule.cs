using EShop.Administration.Localization;
using EShop.Account.Admin;
using Leopard.Identity;
using Leopard.Saas;
using Localization.Resources.AbpUi;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;

namespace EShop.Administration
{
    [DependsOn(
        typeof(AdministrationApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(LeopardSaasHttpApiModule),
        typeof(EShopAccountAdminHttpApiModule),
        typeof(LeopardIdentityHttpApiModule)
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
