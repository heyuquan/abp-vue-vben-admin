using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Leopard.Account.Localization;
using Leopard.Identity;
using Leopard.Base.Shared;

namespace Leopard.Account
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(LeopardIdentityApplicationContractsModule)
    )]
    public class LeopardAccountSharedApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<LeopardAccountSharedApplicationContractsModule>(null, null));
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<LeopardAccountResource>("zh-Hans")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/Leopard/Account/Localization/Resources");
            });
            Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace(ModuleNames.Account, typeof(LeopardAccountResource)));
        }
    }
}
