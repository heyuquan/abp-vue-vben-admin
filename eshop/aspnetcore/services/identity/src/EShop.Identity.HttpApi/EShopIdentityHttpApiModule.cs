using EShop.Identity.Localization;
using Leopard.Identity;
using Localization.Resources.AbpUi;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace EShop.Identity;

[DependsOn(
    typeof(EShopIdentityApplicationContractsModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(LeopardIdentityHttpApiModule)
    )]
public class EShopIdentityHttpApiModule : AbpModule
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
                .Get<IdentityResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
