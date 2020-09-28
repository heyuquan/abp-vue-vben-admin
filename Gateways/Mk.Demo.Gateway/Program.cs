using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Mk.Demo.Gateway
{
    public class Program
    {
        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static int Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                Log.Information("Starting web host.");
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

        private static void ConfigureLogging()
        {
            var cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();

            const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.FFF} {Level:w3}] {Message} {NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Environment", env)
                .Enrich.WithProperty("ProjectName", "Mk.Demo.Gateway")
                //.WriteTo.Debug()
#if DEBUG
                .WriteTo.Console()  // 在容器中，有时候挂载日志文件异常，导致查不出原因，会需要将日志打印到控制台上
#endif
                .WriteTo.Async(c => c.File(
                                        "Logs/logs.txt"
                                        , encoding: Encoding.UTF8
                                        , rollOnFileSizeLimit: true
                                        , fileSizeLimitBytes: (1024 * 10) * 1024    // 10k*1024=10M  最大单个文件10M
                                        , rollingInterval: RollingInterval.Day
                                        , outputTemplate: outputTemplate
                                        )
                                    )
                .WriteTo.Elasticsearch(ConfigureElasticSink(cfg, env))
                .ReadFrom.Configuration(cfg)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot cfg, string env)
        {
            return new ElasticsearchSinkOptions(new Uri(cfg["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                // IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{env?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                IndexFormat = $"Mk.Demo.Gateway-v1-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config)=> {
                    config.AddJsonFile($"ocelot.{env}.json", optional: true, reloadOnChange: true);
                })
                .UseAutofac()
                .UseSerilog();
        }
    }
}
