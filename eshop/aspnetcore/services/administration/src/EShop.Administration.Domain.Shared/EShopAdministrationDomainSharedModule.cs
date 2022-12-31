using EShop.Administration.Localization;
using Leopard.Base.Shared;
using Leopard.Saas;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EShop.Administration
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(LeopardSaasDomainSharedModule)
        )]
    public class EShopAdministrationDomainSharedModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<EShopAdministrationDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AdministrationResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("EShop/Administration/Localization/Resources");

                options.Resources
                    .Get<AbpFeatureManagementResource>()
                    .AddVirtualJson("EShop/Administration/Localization/Feature");

                options.DefaultResourceType = typeof(AdministrationResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(ModuleNames.Administration, typeof(AdministrationResource));
            });
        }
    }
}
