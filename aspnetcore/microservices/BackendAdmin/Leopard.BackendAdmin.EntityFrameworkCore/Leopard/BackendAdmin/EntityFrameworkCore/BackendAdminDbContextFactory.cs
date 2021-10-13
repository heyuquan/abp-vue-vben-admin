using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Leopard.BackendAdmin.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class BackendAdminDbContextFactory : IDesignTimeDbContextFactory<BackendAdminDbContext>
    {
        public BackendAdminDbContext CreateDbContext(string[] args)
        {
            BackendAdminEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var version = new MySqlServerVersion(new System.Version(5, 7, 33));
            var builder = new DbContextOptionsBuilder<BackendAdminDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), version);

            return new BackendAdminDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Leopard.BackendAdmin.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
