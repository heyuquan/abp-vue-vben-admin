using Hangfire;
using Microsoft.Extensions.Logging;
using Mk.DemoB.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace Mk.DemoB.BackgroundJobAppService
{
    /// <summary>
    /// 写业务的服务，服务中使用 IBackgroundJobManager 注入job
    /// IBackgroundJobManager 使用 IBackgroundJobStore 来存储job作业，默认的IBackgroundJobStore为存储为内存
    /// 通过 Volo.Abp.BackgroundJobs.HangFire 包，让其进入hangfire数据库，由hangfire调度
    /// </summary>
    public class BusinessAppService : DemoBAppService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        public readonly ILogger<BusinessAppService> _logger;

        public BusinessAppService(
            IBackgroundJobManager backgroundJobManager
            , ILogger<BusinessAppService> logger
            )
        {
            _backgroundJobManager = backgroundJobManager;
            _logger = logger;
        }

        public async Task RegisterUserAsync()
        {
            // 注册用户..
            // to do ....

            // 创建job作业，eg：发邮件
            await _backgroundJobManager.EnqueueAsync(
                args: new SimpleAbpArgs { BusinessName = "RegisterUser", CreateTime = DateTime.Now }
                , delay: new TimeSpan(0, 0, 5)
                );
        }

        public async Task HangfireRecurringJob()
        {
            RecurringJob.AddOrUpdate("定时任务测试", () => Console.WriteLine("Transparent!"), CronType.Minute());

            await Task.CompletedTask;
        }
    }
}
