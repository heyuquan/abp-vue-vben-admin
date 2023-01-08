using EShop.Identity.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace EShop.Identity;

[DependsOn(
    typeof(IdentityApplicationContractsModule),
    typeof(AbpIdentityHttpApiModule)
    )]
public class IdentityHttpApiModule : AbpModule
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
