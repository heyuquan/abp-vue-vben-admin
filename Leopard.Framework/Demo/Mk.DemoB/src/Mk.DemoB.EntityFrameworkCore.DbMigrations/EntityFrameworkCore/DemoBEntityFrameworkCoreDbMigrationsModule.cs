using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Mk.DemoB.EntityFrameworkCore
{
    [DependsOn(
        typeof(DemoBEntityFrameworkCoreModule)
        )]
    public class DemoBEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DemoBMigrationsDbContext>();
        }
    }
}
