using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Leopard.Account.Localization;

namespace Leopard.Account
{
    [DependsOn(
        typeof(LeopardAccountPublicApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class LeopardAccountPublicHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(LeopardAccountPublicHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<LeopardAccountResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
