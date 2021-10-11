using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Leopard.Saas.EntityFrameworkCore
{
    [DependsOn(
        typeof(SaasDomainModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
    )]
    public class SaasEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SaasDbContext>(options =>
            {
                options.AddRepository<Tenant, EfCoreTenantRepository>();
                options.AddRepository<Edition, EfCoreEditionRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<SaasDbContext>(c =>
                {
                    c.UseSqlServer(b => { b.MigrationsHistoryTable("__Saas_Migrations"); });
                });
            });
        }
    }
}