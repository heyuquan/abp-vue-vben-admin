using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using EShop.Account.Localization;

namespace EShop.Account.Admin
{
    [DependsOn(
        typeof(EShopAccountAdminApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class EShopAccountAdminHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(EShopAccountAdminHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<EShopAccountResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
