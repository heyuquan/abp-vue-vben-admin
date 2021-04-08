using Leopard.Abp.IdentityServer.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;

namespace Leopard.Abp.IdentityServer.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class LeopardIdentityServerEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IIdentityServerBuilder>(
                builder =>
                {
                    builder.AddAbpStores();
                }
            );
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardIdentityServerDbContext>(options =>
            {
                options.AddDefaultRepositories<LeopardIdentityServerDbContext>();

                options.AddRepository<Client, ClientRepository>();
                options.AddRepository<ApiResource, ApiResourceRepository>();
                options.AddRepository<ApiScope, ApiScopeRepository>();
                options.AddRepository<IdentityResource, IdentityResourceRepository>();
                options.AddRepository<PersistedGrant, PersistentGrantRepository>();
                options.AddRepository<DeviceFlowCodes, DeviceFlowCodesRepository>();
            });

            context.Services.TryAddTransient(typeof(IClientRepository), typeof(ClientRepository));
            context.Services.TryAddTransient(typeof(IApiResourceRepository), typeof(ApiResourceRepository));
            context.Services.TryAddTransient(typeof(IApiScopeRepository), typeof(ApiScopeRepository));
            context.Services.TryAddTransient(typeof(IIdentityResourceRepository), typeof(IdentityResourceRepository));
            context.Services.TryAddTransient(typeof(IPersistentGrantRepository), typeof(PersistentGrantRepository));
            context.Services.TryAddTransient(typeof(IDeviceFlowCodesRepository), typeof(DeviceFlowCodesRepository));
        }
    }
}
