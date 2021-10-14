using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Leopard.Saas;
using Leopard.Account.Admin;
using Leopard.Identity;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(BackendAdminApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(AbpFeatureManagementHttpApiClientModule),
        typeof(AbpSettingManagementHttpApiClientModule),
        typeof(SaasHttpApiClientModule),
        typeof(LeopardAccountAdminHttpApiClientModule),
        typeof(LeopardIdentityHttpApiClientModule)
    )]
    public class BackendAdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "BackendAdmin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BackendAdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
