using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Leopard.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpIdentityDomainModule), 
        typeof(AbpUsersEntityFrameworkCoreModule))]
    public class LeopardIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardIdentityDbContext>(options =>
            {
                options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
                options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
                options.AddRepository<IdentityClaimType, EfCoreIdentityClaimTypeRepository>();
                options.AddRepository<OrganizationUnit, EfCoreOrganizationUnitRepository>();
                options.AddRepository<IdentitySecurityLog, EFCoreIdentitySecurityLogRepository>();
                options.AddRepository<IdentityLinkUser, EfCoreIdentityLinkUserRepository>();
            });

            context.Services.TryAddTransient(typeof(IIdentityClaimTypeRepository), typeof(EfCoreIdentityClaimTypeRepository));
            context.Services.TryAddTransient(typeof(IIdentityRoleRepository), typeof(EfCoreIdentityRoleRepository));
            context.Services.TryAddTransient(typeof(IIdentityUserRepository), typeof(EfCoreIdentityUserRepository));
            context.Services.TryAddTransient(typeof(IOrganizationUnitRepository), typeof(EfCoreOrganizationUnitRepository));
            context.Services.TryAddTransient(typeof(IIdentitySecurityLogRepository), typeof(EFCoreIdentitySecurityLogRepository));
            context.Services.TryAddTransient(typeof(IIdentityLinkUserRepository), typeof(EfCoreIdentityLinkUserRepository));
        }
    }
}
