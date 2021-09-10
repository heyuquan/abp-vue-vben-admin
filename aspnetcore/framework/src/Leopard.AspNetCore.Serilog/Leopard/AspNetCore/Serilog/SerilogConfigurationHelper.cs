using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.AspNetCore.Serilog
{
    public static class SerilogConfigurationHelper
    {
        public static void Configure(string env, string applicationName, bool isWriteToFile, bool isWriteToElasticsearch)
        {
            var cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();

            // https://www.cnblogs.com/Quinnz/p/12202633.html
            const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.FFF} {Level:w3}] {Message} {NewLine}{Exception}";

            var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            //.Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", env)
            .Enrich.WithProperty("ProjectName", applicationName)
#if DEBUG
            .WriteTo.Async(c => c.Console());  // 在容器中，有时候挂载日志文件异常，导致查不出原因，会需要将日志打印到控制台上
#endif
            if (isWriteToFile)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Async(c => c.File(
                                "Logs/logs.txt"
                                , encoding: Encoding.UTF8
                                , rollOnFileSizeLimit: true
                                , fileSizeLimitBytes: (1024 * 10) * 1024    // 10k*1024=10M  最大单个文件10M
                                , rollingInterval: RollingInterval.Day
                                , outputTemplate: outputTemplate
                                )
                            );
            }
            if (isWriteToElasticsearch)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Elasticsearch(ConfigureElasticSink(cfg, env, applicationName));
            }

            Log.Logger = loggerConfiguration.ReadFrom.Configuration(cfg).CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot cfg, string env, string applicationName)
        {
            return new ElasticsearchSinkOptions(new Uri(cfg["ElasticSearch:Uri"]))
            {
                AutoRegisterTemplate = true,
                // IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{env?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                // 加入版本 v1 若索引字段有变化，需要重建索引，就可以根据版本重新建一个新的索引
                IndexFormat = $"{applicationName}-v1-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
