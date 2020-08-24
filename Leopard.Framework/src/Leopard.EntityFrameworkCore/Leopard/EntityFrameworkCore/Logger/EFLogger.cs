using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Leopard.EntityFrameworkCore.Logger
{

    // 方案来源：https://www.cnblogs.com/lwqlun/p/13551149.html
    // 在10楼评论，直接改.net core配置文件即可实现打印ef sql ，
    // 如下 ，但这边还是保留 EFLogger 实现方案。可做学习或者做更多日志输出的控制
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
        private readonly IConfiguration _config;

        public EFLogger(
            ILogger<EFLogger> logger
            , IConfiguration config
            )
        {
            _logger = logger;
            _config = config;
        }

        public void SetCategoryName(string categoryName)
        {
            this.categoryName = categoryName;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            bool isEnable;
            bool.TryParse(_config.GetSection("EFCore:IsEnableLog").Value, out isEnable);
            return isEnable;
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
                _logger.LogInformation(logContent);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
