using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.DemoB.BackgroundJobs.Job;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace Mk.DemoB.BackgroundJobs
{
    // 
    [DependsOn(
        typeof(AbpBackgroundJobsModule)
        // 先去掉hangfire后台任务，不然hangfire一直打日志
        // typeof(AbpBackgroundJobsHangfireModule)
        , typeof(DemoBDomainModule)
        )]

    public class DemoBBackgroundJobsModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            // 先去掉hangfire后台任务，不然hangfire一直打日志
            //GlobalConfiguration.Configuration.UseStorage(
            //       new MySqlStorage(
            //           configuration.GetConnectionString("Default")
            //           , new MySqlStorageOptions
            //           {
            //               TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            //               QueuePollInterval = TimeSpan.FromSeconds(15),
            //               JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //               CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //               PrepareSchemaIfNecessary = true,
            //               DashboardJobListLimit = 50000,
            //               TransactionTimeout = TimeSpan.FromMinutes(1),
            //               TablesPrefix = "Hangfire_"
            //           }
            //       ));
            //TransactionIsolationLevel - transaction isolation level. Default is read committed.
            //QueuePollInterval - job queue polling interval.Default is 15 seconds.
            //JobExpirationCheckInterval - job expiration check interval(manages expired records).Default is 1 hour.
            //CountersAggregateInterval - interval to aggregate counter.Default is 5 minutes.
            //PrepareSchemaIfNecessary - if set to true, it creates database tables. Default is true.
            //DashboardJobListLimit - dashboard job list limit.Default is 50000.
            //TransactionTimeout - transaction timeout.Default is 1 minute.
            //TablesPrefix - prefix for the tables in database.Default is none

            //// 注册dotnet core 后台服务  结合Abp使用会出问题，参考 CaptureExechangeRateJob.cs
            context.Services.AddTransient<IHostedService, SimpleDotNetJob>();

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();

            // 先去掉hangfire后台任务，不然hangfire一直打日志
            //app.UseHangfireServer();

            //app.UseHangfireDashboard(options: new DashboardOptions
            //{
            //    Authorization = new[]
            //    {
            //        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            //        {
            //            RequireSsl = false,
            //            SslRedirect = false,
            //            LoginCaseSensitive = true,
            //            Users = new []
            //            {
            //                new BasicAuthAuthorizationUser
            //                {
            //                    Login = configuration["Hangfire:Login"],
            //                    PasswordClear =  configuration["Hangfire:Password"]
            //                }
            //            }
            //        })
            //    },
            //    DashboardTitle = "任务调度中心"
            //});

            context.AddBackgroundWorker<CaptureExechangeRateWorker>();
        }
    }
}
