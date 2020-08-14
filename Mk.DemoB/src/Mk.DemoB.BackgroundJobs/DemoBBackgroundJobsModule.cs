using System;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.BackgroundJobs.Hangfire;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.Hosting;

namespace Mk.DemoB.BackgroundJobs
{
    [DependsOn(typeof(AbpBackgroundJobsHangfireModule))]
    public class DemoBBackgroundJobsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            GlobalConfiguration.Configuration.UseStorage(
                   new MySqlStorage(
                       configuration.GetConnectionString("Default")
                       , new MySqlStorageOptions
                       {
                           TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                           QueuePollInterval = TimeSpan.FromSeconds(15),
                           JobExpirationCheckInterval = TimeSpan.FromHours(1),
                           CountersAggregateInterval = TimeSpan.FromMinutes(5),
                           PrepareSchemaIfNecessary = true,
                           DashboardJobListLimit = 50000,
                           TransactionTimeout = TimeSpan.FromMinutes(1),
                           TablesPrefix = "Hangfire_"
                       }
                   ));
            //TransactionIsolationLevel - transaction isolation level. Default is read committed.
            //QueuePollInterval - job queue polling interval.Default is 15 seconds.
            //JobExpirationCheckInterval - job expiration check interval(manages expired records).Default is 1 hour.
            //CountersAggregateInterval - interval to aggregate counter.Default is 5 minutes.
            //PrepareSchemaIfNecessary - if set to true, it creates database tables. Default is true.
            //DashboardJobListLimit - dashboard job list limit.Default is 50000.
            //TransactionTimeout - transaction timeout.Default is 1 minute.
            //TablesPrefix - prefix for the tables in database.Default is none

            //// 注册dotnet core 后台服务
            context.Services.AddTransient<IHostedService, SimpleDotNetJob>();

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();

            app.UseHangfireServer();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new []
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = configuration["Hangfire:Login"],
                                PasswordClear =  configuration["Hangfire:Password"]
                            }
                        }
                    })
                },
                DashboardTitle = "任务调度中心"
            });

        }
    }
}
