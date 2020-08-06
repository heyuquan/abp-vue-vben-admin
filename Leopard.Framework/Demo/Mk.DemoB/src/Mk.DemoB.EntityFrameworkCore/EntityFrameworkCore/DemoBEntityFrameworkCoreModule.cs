using Microsoft.Extensions.DependencyInjection;
using Leopard.Abp.AuditLogging.EntityFrameworkCore;
using Leopard.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Leopard.Abp.FeatureManagement.EntityFrameworkCore;
using Leopard.Abp.Identity.EntityFrameworkCore;
using Leopard.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Leopard.Abp.PermissionManagement.EntityFrameworkCore;
using Leopard.Abp.SettingManagement.EntityFrameworkCore;
using Leopard.Abp.TenantManagement.EntityFrameworkCore;

namespace Mk.DemoB.EntityFrameworkCore
{
    [DependsOn(
        typeof(DemoBDomainModule),
        typeof(LeopardIdentityEntityFrameworkCoreModule),
        typeof(LeopardIdentityServerEntityFrameworkCoreModule),
        typeof(LeopardPermissionManagementEntityFrameworkCoreModule),
        typeof(LeopardSettingManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(LeopardBackgroundJobsEntityFrameworkCoreModule),
        typeof(LeopardAuditLoggingEntityFrameworkCoreModule),
        typeof(LeopardTenantManagementEntityFrameworkCoreModule),
        typeof(LeopardFeatureManagementEntityFrameworkCoreModule)
        )]
    public class DemoBEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            DemoBEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DemoBDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also DemoBMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
            });
        }
    }
}
