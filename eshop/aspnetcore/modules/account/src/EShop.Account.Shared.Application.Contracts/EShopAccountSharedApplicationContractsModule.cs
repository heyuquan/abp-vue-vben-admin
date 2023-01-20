using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using EShop.Account.Localization;
using Leopard.Identity;
using EShop.Common.Shared;

namespace EShop.Account
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(LeopardIdentityApplicationContractsModule)
    )]
    public class EShopAccountSharedApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<EShopAccountSharedApplicationContractsModule>(null, null));
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<EShopAccountResource>("zh-Hans")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/EShop/Account/Localization/Resources");
            });
            Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace(ModuleNames.Account, typeof(EShopAccountResource)));
        }
    }
}
