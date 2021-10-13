using Leopard.AspNetCore.Serilog;
using Leopard.BackendAdmin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Leopard.BackendAdmin.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BackendAdminEntityFrameworkCoreModule),
        typeof(BackendAdminApplicationContractsModule),
        typeof(LeopardAspNetCoreSerilogModule)
        )]
    public class BackendAdminDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
