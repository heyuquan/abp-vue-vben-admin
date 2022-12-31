using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mk.DemoB.Data;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoB.EntityFrameworkCore
{
    public class EntityFrameworkCoreDemoBDbSchemaMigrator
        : IDemoBDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreDemoBDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the AuthServerDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<DemoBDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
