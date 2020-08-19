using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mk.DemoB.ExchangeRateMgr;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoB.BackgroundJobs.Job
{
    public class CaptureExechangeRateJob : BackgroundService
    {
        private readonly ILogger<CaptureExechangeRateJob> _logger;
        private readonly ExchangeRateManager _exchangeRateManager;

        public CaptureExechangeRateJob(
            ILogger<CaptureExechangeRateJob> logger
            , ExchangeRateManager exchangeRateManager
            )
        {
            _logger = logger;
            _exchangeRateManager = exchangeRateManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = $"dotnetcore原生后台任务：CurrentTime:{ DateTime.Now}, Hello World!";

                Console.WriteLine(msg);

                _logger.LogInformation(msg);

                await Task.Delay(1000 * 60 * 10, stoppingToken);
            }
        }
    }
}
