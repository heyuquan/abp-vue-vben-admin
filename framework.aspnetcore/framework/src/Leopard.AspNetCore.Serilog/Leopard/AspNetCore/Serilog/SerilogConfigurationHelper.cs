using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
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
            // {Message:lj} 格式选项使消息中嵌入的数据输出在 JSON（j）中，但字符串文本除外，这些文本是原样输出的。
            // {Level:u3} 三个字符大写或 {Level:w3} 小写作为级别名称的格式
            const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.FFF} {Level:w3}] {Message:lj} {NewLine}{Exception}";

            var loggerConfiguration = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            //.Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", env)
            .Enrich.WithProperty("Application", applicationName)
#if DEBUG
            .WriteTo.Async(c => c.Console());  // 在容器中，有时候挂载日志文件异常，导致查不出原因，会需要将日志打印到控制台上
#else
            ;
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
                                , retainedFileCountLimit: 108    // 最多保留最近的108个文件（PS：有时一天能产生几个日志文件）
                                )
                            );
            }
            if (isWriteToElasticsearch)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Elasticsearch(ConfigureElasticSink(cfg, env, applicationName));
            }

            loggerConfiguration.Filter.ByExcluding(logEvent =>
            {
                if (logEvent.Level <= LogEventLevel.Information)
                {
                    logEvent.Properties.TryGetValue("RequestPath", out LogEventPropertyValue requestPathValue);
                    if (requestPathValue != null
                        && String.Compare(requestPathValue.ToString().Trim('"'), "/api/health", true) == 0)
                    {
                        // 过滤掉 健康检查 的 Verbose、Debug、Info 日志，因为健康检查打的日志太多了
                        // 如果是异常的信息，则要显示出来
                        return true;
                    }
                }

                return false;
            });

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
