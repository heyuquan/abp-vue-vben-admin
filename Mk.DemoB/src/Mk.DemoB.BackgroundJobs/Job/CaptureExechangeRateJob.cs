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

namespace Mk.DemoB.BackgroundJobs.Job
{
    // 注意：net core 的 BackgroundService 不能直接用于AbpVnext中
    // 原因：直接使用，依赖注入会出现问题。eg：在注入 IRepository 后，使用时会报对应的dbcontext已经被释放
    // 所以：要使用 AsyncPeriodicBackgroundWorkerBase  https://docs.abp.io/zh-Hans/abp/latest/Background-Workers
    // ??????

    public class CaptureExechangeRateJob : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly ILogger<CaptureExechangeRateJob> _logger;
        //private const int IntervalSecond = (1000 * 60) * 60 * 8; //8小时抓取一次
        private const int IntervalSecond = (1000 * 10); //6小时抓取一次
        private const int CaptureCountPerDay = 3;   // 每天抓取多少次

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

        protected override async Task DoWorkAsync(
                PeriodicBackgroundWorkerContext workerContext)
        {
            var exchangeRateCaptureBatchRepository = workerContext
                .ServiceProvider
                .GetRequiredService<IRepository<ExchangeRateCaptureBatch, Guid>>();

            var exchangeRateCaptureBatchs = exchangeRateCaptureBatchRepository
                .Where(x => x.IsSuccess == true)
                .OrderByDescending(x => x.Id)
                .Take(CaptureCountPerDay)
                .ToList();

            int hadCapture = 0;
            if (exchangeRateCaptureBatchs.Any())
            {
                DateTime now = DateTime.Now;
                foreach (var item in exchangeRateCaptureBatchs)
                {
                    DateTime capturetime = exchangeRateCaptureBatchs.First().CaptureTime;
                    if (capturetime.Date.Equals(now.Date))
                    {
                        // 同一天就计数
                        hadCapture++;
                    }
                }
            }

            if (CaptureCountPerDay > hadCapture)
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
