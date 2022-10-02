using Mk.DemoB.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Mk.DemoB.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(DemoBEntityFrameworkCoreModule),
        typeof(DemoBApplicationContractsModule)
        )]
    public class DemoBDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
