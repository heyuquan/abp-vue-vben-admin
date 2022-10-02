using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Leopard.EntityFrameworkCore.Logger
{

    // 方案来源：https://www.cnblogs.com/lwqlun/p/13551149.html
    // 在10楼评论，直接改.net core配置文件即可实现打印ef sql ，
    // 如下 ，但这边还是保留 EFLogger 实现方案。可做学习或者做更多日志输出的控制（比如控制耗时长的进行记录）
    //"Logging": {
    //    "LogLevel": {
    //      "Default": "Information",
    //      "Override": {
    //        "Microsoft.EntityFrameworkCore.Database": "Information"
    //      }
    //    }
    //  },


    public class EFLogger : ILogger
    {
        private string categoryName;
        private readonly ILogger<EFLogger> _logger;
        private readonly EFLogOptions _efLogOptions;

        public EFLogger(
            ILogger<EFLogger> logger
            , IOptions<EFLogOptions> efLogOptions
            )
        {
            _logger = logger;
            _efLogOptions = efLogOptions.Value;
        }

        public void SetCategoryName(string categoryName)
        {
            this.categoryName = categoryName;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _efLogOptions.IsEnableLog;
        }

        public void Log<TState>(
            LogLevel logLevel
            , EventId eventId
            , TState state
            , Exception exception
            , Func<TState, Exception, string> formatter)
        {
            // //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command" &&
                logLevel == LogLevel.Information)
            {
                string logContent = formatter(state, exception);

                if (_efLogOptions.ExecuteTimeSpent > 0)
                {
                    var values = state as IReadOnlyList<KeyValuePair<string, object>>;
                    string timeSpentStr = values.First(p => p.Key == "elapsed").Value.ToString();
                    // 去掉 ,    因为如  1325ms表示为：1.325ms
                    timeSpentStr = timeSpentStr.Replace(",", String.Empty);
                    var timeSpent = Convert.ToInt32(timeSpentStr);

                    if (timeSpent > _efLogOptions.ExecuteTimeSpent)
                    {
                        _logger.LogWarning(logContent);
                    }
                }
                else
                {
                    _logger.LogInformation(logContent);
                }
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
