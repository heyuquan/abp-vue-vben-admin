using Leopard.AspNetCore.Serilog;
using SSO.AuthServer.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace SSO.AuthServer.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AuthServerEntityFrameworkCoreModule),
        typeof(AuthServerApplicationContractsModule),
        typeof(LeopardAspNetCoreSerilogModule)
        )]
    public class AuthServerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
