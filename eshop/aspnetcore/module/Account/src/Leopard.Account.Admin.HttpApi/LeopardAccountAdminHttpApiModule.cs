using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Leopard.Account.Localization;

namespace Leopard.Account.Admin
{
    [DependsOn(
        typeof(LeopardAccountAdminApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class LeopardAccountAdminHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(LeopardAccountAdminHttpApiModule).Assembly);
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
