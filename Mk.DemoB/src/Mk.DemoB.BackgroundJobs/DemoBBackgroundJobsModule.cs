using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.DemoB.BackgroundJobs.Job;
using System;
using System.Collections.Generic;
using System.Transactions;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace Mk.DemoB.BackgroundJobs
{
    // 
    [DependsOn(
        typeof(AbpBackgroundJobsModule)
        , typeof(AbpBackgroundJobsHangfireModule)
        , typeof(DemoBDomainModule)
        )]

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
                           TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事务隔离级别。默认是读取已提交。
                           QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                           JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                           CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                           PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                           DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
                           TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                           TablesPrefix = "hangfire_"                                  //- 数据库中表的前缀。默认为none
                       }
                   ));

            //// 注册dotnet core 后台服务  结合Abp使用会出问题，参考 CaptureExechangeRateJob.cs
            context.Services.AddTransient<IHostedService, SimpleDotNetJob>();

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();

            app.UseHangfireServer();

            var hangfireStartUpPath = "/job";
            app.UseHangfireDashboard(
                pathMatch: hangfireStartUpPath,
                options: new DashboardOptions
                {
                    AppPath = "#",
                    DisplayStorageConnectionString = false,
                    IsReadOnlyFunc = Context => false,
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

            var hangfireReadOnlyPath = "/job-read";
            //只读面板，只能读取不能操作
            app.UseHangfireDashboard(hangfireReadOnlyPath, new DashboardOptions
            {
                IgnoreAntiforgeryToken = true,//这里一定要写true 不然用client库写代码添加webjob会出错
                AppPath = hangfireStartUpPath,//返回时跳转的地址
                DisplayStorageConnectionString = false,//是否显示数据库连接信息
                IsReadOnlyFunc = Context => true
            });

            context.AddBackgroundWorker<CaptureExechangeRateWorker>();

            //支持基于队列的任务处理：任务执行不是同步的，而是放到一个持久化队列中，以便马上把请求控制权返回给调用者。
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("队列任务执行了！"));

            //延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行，延迟作业仅执行一次
            // var jobId = BackgroundJob.Schedule(() => Console.WriteLine("一天后的延迟任务执行了！"),TimeSpan.FromDays(1));//一天后执行该任务

            //循环任务执行：一行代码添加重复执行的任务，其内置了常见的时间循环模式，也可基于CRON表达式来设定复杂的模式。【用的比较的多】
            RecurringJob.AddOrUpdate(() => Console.WriteLine("每小时执行任务！"), Cron.Hourly); //注意最小单位是分钟

            //延续性任务执行：类似于.NET中的Task,可以在第一个任务执行完之后紧接着再次执行另外的任务
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("连续任务！"));
        }
    }
}
