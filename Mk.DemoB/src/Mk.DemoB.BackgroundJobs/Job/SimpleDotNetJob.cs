using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace Mk.DemoB.BackgroundJobs
{
    // 使用.net core中内置的实现方式
    public class SimpleDotNetJob : BackgroundService
    {
        private readonly ILogger<SimpleDotNetJob> _logger;
        private readonly Clock _clock;

        public SimpleDotNetJob(
            ILogger<SimpleDotNetJob> logger
            , Clock clock
            )
        {
            _logger = logger;
            _clock = clock;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = $"dotnetcore原生后台任务：CurrentTime:{ _clock.Now}, Hello World!";

                Console.WriteLine(msg);

                _logger.LogInformation(msg);

                await Task.Delay(1000*60*10, stoppingToken);
            }
        }
    }
}
