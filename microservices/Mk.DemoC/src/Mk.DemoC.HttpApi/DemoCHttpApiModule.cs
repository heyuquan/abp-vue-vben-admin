using Localization.Resources.AbpUi;
using Mk.DemoC.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class DemoCHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(DemoCHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<DemoCResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
