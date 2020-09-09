using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Mk.DemoC
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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Environment", env)
                //.WriteTo.Debug()
                .WriteTo.Console()  // 在容器中，有时候挂载日志文件异常，导致查不出原因，会需要将日志打印到控制台上
                //.WriteTo.Async(c => c.File("Logs/logs.txt"))
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
                IndexFormat = $"Mk.DemoC.Api-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
        }
    }
}
