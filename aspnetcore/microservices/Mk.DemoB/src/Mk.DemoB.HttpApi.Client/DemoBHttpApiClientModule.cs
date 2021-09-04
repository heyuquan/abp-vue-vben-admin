using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(DemoBApplicationContractsModule)
        //typeof(AbpAccountHttpApiClientModule),
        //typeof(AbpIdentityHttpApiClientModule),
        //typeof(AbpPermissionManagementHttpApiClientModule),
        //typeof(AbpTenantManagementHttpApiClientModule),
        //typeof(AbpFeatureManagementHttpApiClientModule)
    )]
    public class DemoBHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "DemoB";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(DemoBApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
