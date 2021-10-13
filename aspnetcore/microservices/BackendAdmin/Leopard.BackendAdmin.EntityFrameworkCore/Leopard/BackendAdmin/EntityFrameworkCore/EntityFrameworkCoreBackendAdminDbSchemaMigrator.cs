﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Leopard.BackendAdmin.Data;
using Volo.Abp.DependencyInjection;

namespace Leopard.BackendAdmin.EntityFrameworkCore
{
    public class EntityFrameworkCoreBackendAdminDbSchemaMigrator
        : IBackendAdminDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreBackendAdminDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the BackendAdminDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<BackendAdminDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
