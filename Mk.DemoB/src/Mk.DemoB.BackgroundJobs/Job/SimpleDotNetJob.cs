using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mk.DemoB.BackgroundJobs
{
    // 使用.net core中内置的实现方式
    public class SimpleDotNetJob : BackgroundService
    {
        public readonly ILogger<SimpleDotNetJob> _logger;

        public SimpleDotNetJob(ILogger<SimpleDotNetJob> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = $"dotnetcore原生后台任务：CurrentTime:{ DateTime.Now}, Hello World!";

                Console.WriteLine(msg);

                _logger.LogInformation(msg);

                await Task.Delay(1000*60*10, stoppingToken);
            }
        }
    }
}
