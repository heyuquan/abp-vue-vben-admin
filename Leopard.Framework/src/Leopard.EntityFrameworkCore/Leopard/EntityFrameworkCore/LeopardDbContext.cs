using Leopard.EntityFrameworkCore.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Leopard.EntityFrameworkCore
{
    public class LeopardDbContext<TDbContext> : AbpDbContext<TDbContext>, ITransientDependency where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        public LeopardDbContext(
            DbContextOptions<TDbContext> options
            ,IServiceProvider serviceProvider
            ): base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new LoggerFactory();
            EFLoggerProvider _efLoggerProvider = _serviceProvider.GetService<EFLoggerProvider>();
            loggerFactory.AddProvider(_efLoggerProvider);
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.UseLoggerFactory(loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
