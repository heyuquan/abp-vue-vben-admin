using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class AuthServerApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AuthServerDtoExtensions.Configure();
        }
    }
}
