using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Leopard.EntityFrameworkCore.Logger
{
    public class EFLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public EFLoggerProvider(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            EFLogger logger = _serviceProvider.GetService<EFLogger>();
            logger.SetCategoryName(categoryName);
            return logger;
        }

        public void Dispose() { }
    }
}
