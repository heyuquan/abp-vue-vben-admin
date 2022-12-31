using Leopard.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace Leopard.Saas.EntityFrameworkCore
{
    [DependsOn(
        typeof(LeopardSaasDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(LeopardEntityFrameworkCoreModule)
    )]
    public class LeopardSaasEntityFrameworkCoreModule : AbpModule
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
                options.UseMySQL();
            });
        }
    }
}