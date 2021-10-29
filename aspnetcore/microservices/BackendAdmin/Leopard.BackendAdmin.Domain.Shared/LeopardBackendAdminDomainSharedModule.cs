using Leopard.BackendAdmin.Localization;
using Leopard.Saas;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(LeopardSaasDomainSharedModule)
        )]
    public class LeopardBackendAdminDomainSharedModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<LeopardBackendAdminDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BackendAdminResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("Leopard/BackendAdmin/Localization/Resources");

                options.DefaultResourceType = typeof(BackendAdminResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("BackendAdmin", typeof(BackendAdminResource));
            });
        }
    }
}
