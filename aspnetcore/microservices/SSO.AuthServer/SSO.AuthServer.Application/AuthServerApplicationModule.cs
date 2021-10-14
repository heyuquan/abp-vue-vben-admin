using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(AuthServerApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule)
        )]
    public class AuthServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AuthServerApplicationModule>();
            });
        }
    }
}
