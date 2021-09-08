using Leopard.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace Mk.DemoB.EntityFrameworkCore
{
    [DependsOn(
        typeof(DemoBDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(LeopardEntityFrameworkCoreModule)
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
