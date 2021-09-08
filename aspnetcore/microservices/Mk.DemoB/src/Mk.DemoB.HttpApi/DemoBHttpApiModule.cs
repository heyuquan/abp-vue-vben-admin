using Localization.Resources.AbpUi;
using Mk.DemoB.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBApplicationContractsModule)
        )]
    public class DemoBHttpApiModule : AbpModule
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
                    .Get<DemoBResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
