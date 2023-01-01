using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using EShop.Account.Localization;

namespace EShop.Account.Public
{
    [DependsOn(
        typeof(EShopAccountPublicApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class EShopAccountPublicHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(EShopAccountPublicHttpApiModule).Assembly);
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
