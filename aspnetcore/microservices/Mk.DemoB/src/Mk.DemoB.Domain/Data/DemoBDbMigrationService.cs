using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Mk.DemoB.Data
{
    public class DemoBDbMigrationService : ITransientDependency
    {
        public ILogger<DemoBDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IDemoBDbSchemaMigrator> _dbSchemaMigrators;
        private readonly ICurrentTenant _currentTenant;

        public DemoBDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IDemoBDbSchemaMigrator> dbSchemaMigrators,
            ICurrentTenant currentTenant)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;
            _currentTenant = currentTenant;

            Logger = NullLogger<DemoBDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            try
            {
                if (DbMigrationsProjectExists() && !MigrationsFolderExists())
                {
                    AddInitialMigration();
                    return;
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning("Couldn't determinate if any migrations exist : " + e.Message);
            }

            Logger.LogInformation("Started database migrations...");

            await MigrateDatabaseSchemaAsync();
            await SeedDataAsync();

            Logger.LogInformation($"Successfully completed host database migrations.");

            //var tenants = await _tenantAppService.GetListAsync(new GetTenantsInput());

            //var migratedDatabaseSchemas = new HashSet<string>();
            //foreach (var tenant in tenants.Items)
            //{
            //    using (_currentTenant.Change(tenant.Id))
            //    {
            //        if (tenant.ConnectionStrings.Any())
            //        {
            //            var tenantConnectionStrings = tenant.ConnectionStrings
            //                .Select(x => x.Value)
            //                .ToList();

            //            if (!migratedDatabaseSchemas.IsSupersetOf(tenantConnectionStrings))
            //            {
            //                await MigrateDatabaseSchemaAsync(tenant);

            //                migratedDatabaseSchemas.AddIfNotContains(tenantConnectionStrings);
            //            }
            //        }

            //        await SeedDataAsync(tenant);
            //    }

            //    Logger.LogInformation($"Successfully completed {tenant.Name} tenant database migrations.");
            //}

            //Logger.LogInformation("Successfully completed all database migrations.");
            //Logger.LogInformation("You can safely end this process...");
        }

        private async Task MigrateDatabaseSchemaAsync(TenantDto tenant = null)
        {
            Logger.LogInformation(
                $"Migrating schema for {(tenant == null ? "host" : tenant.Name + " tenant")} database...");

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync(TenantDto tenant = null)
        {
            Logger.LogInformation($"Executing {(tenant == null ? "host" : tenant.Name + " tenant")} database seed...");

            await _dataSeeder.SeedAsync(tenant?.Id);
        }

        private bool DbMigrationsProjectExists()
        {
            var dbMigrationsProjectFolder = GetDbMigrationsProjectFolderPath();

            return dbMigrationsProjectFolder != null;
        }

        private bool MigrationsFolderExists()
        {
            var dbMigrationsProjectFolder = GetDbMigrationsProjectFolderPath();

            return Directory.Exists(Path.Combine(dbMigrationsProjectFolder, "migrations"));
        }

        private void AddInitialMigration()
        {
            Logger.LogInformation("Creating initial migration...");

            string argumentPrefix;
            string fileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                argumentPrefix = "-c";
                fileName = "/bin/bash";
            }
            else
            {
                argumentPrefix = "/C";
                fileName = "cmd.exe";
            }

            var procStartInfo = new ProcessStartInfo(fileName,
                $"{argumentPrefix} \"abp create-migration-and-run-migrator \"{GetDbMigrationsProjectFolderPath()}\"\""
            );

            try
            {
                Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                throw new Exception("Couldn't run ABP CLI...");
            }
        }

        private string GetDbMigrationsProjectFolderPath()
        {
            var slnDirectoryPath = GetSolutionDirectoryPath();

            if (slnDirectoryPath == null)
            {
                throw new Exception("Solution folder not found!");
            }

            var srcDirectoryPath = Path.Combine(slnDirectoryPath, "src");

            return Directory.GetDirectories(srcDirectoryPath)
                .FirstOrDefault(d => d.EndsWith(".EntityFrameworkCore"));
        }

        private string GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);

                if (Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln")) != null)
                {
                    return currentDirectory.FullName;
                }
            }

            return null;
        }
    }
}