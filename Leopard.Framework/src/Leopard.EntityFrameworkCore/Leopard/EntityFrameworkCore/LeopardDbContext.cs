using Leopard.EntityFrameworkCore.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Leopard.EntityFrameworkCore
{
    public class LeopardDbContext<TDbContext> : AbpDbContext<TDbContext>, ITransientDependency where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EFLogOptions _efLogOptions;

        public LeopardDbContext(
            DbContextOptions<TDbContext> options
            , IServiceProvider serviceProvider
            ) : base(options)
        {
            _serviceProvider = serviceProvider;
            _efLogOptions = serviceProvider.GetService<IOptions<EFLogOptions>>().Value;
        }

        public override void Initialize(AbpEfCoreDbContextInitializationContext initializationContext)
        {
            base.Initialize(initializationContext);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new LoggerFactory();
            EFLoggerProvider _efLoggerProvider = _serviceProvider.GetService<EFLoggerProvider>();
            if (_efLoggerProvider != null)
            {
                loggerFactory.AddProvider(_efLoggerProvider);
                optionsBuilder.EnableSensitiveDataLogging(_efLogOptions.EnableSensitiveData);
                optionsBuilder.UseLoggerFactory(loggerFactory);
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
