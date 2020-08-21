using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mk.DemoB.ExchangeRateMgr;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Mk.DemoB.BackgroundJobs.Job
{
    // 注意：net core 的 BackgroundService 不能直接用于AbpVnext中
    // 原因：直接使用，依赖注入会出现问题。eg：在注入 IRepository 后，使用时会报对应的dbcontext已经被释放
    // 1、所以：要使用 AsyncPeriodicBackgroundWorkerBase  https://docs.abp.io/zh-Hans/abp/latest/Background-Workers

    // 仓储使用到的数据库上下文对象是通过工作单元的 IServiceProvider 进行解析的.(即：仓储必须在工作单元中)
    // 2、DoWorkAsync 中使用仓储，必须带上 [UnitOfWork] 特性，手动开启工作单元。否者也会出现DbContext被释放的问题
    // 原因：https://www.cnblogs.com/myzony/p/11647030.html
    // AppService中的方法不用带 [UnitOfWork] 特性，是因为基类实现了Abp自动开启了工作单元。后续再研究abp源码

    // 使用 PeriodicBackgroundWorkerContext 解析依赖 而不是构造函数. 
    // 因为 AsyncPeriodicBackgroundWorkerBase 使用 IServiceScope 在你的任务执行结束时会对其 disposed.

    public class CaptureExechangeRateJob : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly ILogger<CaptureExechangeRateJob> _logger;
        private const int IntervalSecond = (1000 * 60) * 60 * 8; //8小时抓取一次
        private const int CaptureCountPerDay = 1;   // 每天抓取多少次

        public CaptureExechangeRateJob(
                AbpTimer timer
                , IServiceScopeFactory serviceScopeFactory
                , ILogger<CaptureExechangeRateJob> logger
            ) : base(
            timer,
            serviceScopeFactory)
        {
            _logger = logger;
            timer.Period = IntervalSecond;
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync(
                PeriodicBackgroundWorkerContext workerContext)
        {
            var exchangeRateCaptureBatchRepository = workerContext
                .ServiceProvider
                .GetRequiredService<IRepository<ExchangeRateCaptureBatch, Guid>>();

            var exchangeRateCaptureBatchs = await exchangeRateCaptureBatchRepository
                .Where(x => x.IsSuccess == true)
                .OrderByDescending(x => x.Id)
                .Take(CaptureCountPerDay)
                .ToListAsync();

            int hadCaptureCount = 0;
            if (exchangeRateCaptureBatchs.Any())
            {
                DateTime now = DateTime.Now;
                foreach (var item in exchangeRateCaptureBatchs)
                {
                    DateTime capturetime = item.CaptureTime;
                    if (capturetime.Date.Equals(now.Date))
                    {
                        // 同一天就计数
                        hadCaptureCount++;
                    }
                }
            }

            if (CaptureCountPerDay > hadCaptureCount)
            {
                ExchangeRateManager exchangeRateManager = workerContext
                    .ServiceProvider
                    .GetRequiredService<ExchangeRateManager>();
                // 抓取次数不够，就触发抓取
                await exchangeRateManager.CaptureAllRateAndSaveAsync();
            }
        }
    }
}
