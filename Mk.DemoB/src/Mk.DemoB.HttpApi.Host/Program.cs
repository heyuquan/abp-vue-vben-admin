using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;

namespace Mk.DemoB
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //.MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .CreateLogger();

            try
            {
                Log.Information("Starting Mk.DemoB.HttpApi.Host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
                 //.ConfigureLogging((hostingContext, loggingBuilder) =>
                 //{
                 //    // 在容器中运行直接报错了，没有把日志输出到日志。所以有时候需要开启Console，方便查问题
                 //    // 如果开启Console，需要把 UseSerilog() 注释掉
                 //    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                 //    loggingBuilder.AddConsole();
                 //    loggingBuilder.AddDebug();
                 //})
                 .UseAutofac()
                 .UseSerilog();
        }
    }
}
