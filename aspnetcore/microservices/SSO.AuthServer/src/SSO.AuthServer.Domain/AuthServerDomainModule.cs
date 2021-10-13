using Leopard.Buiness.Shared;
using Leopard.Saas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerDomainSharedModule),
        typeof(AbpIdentityDomainModule),        
        typeof(SaasDomainModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpEmailingModule)
    )]
    public class AuthServerDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

#if DEBUG
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
        }
    }
}
