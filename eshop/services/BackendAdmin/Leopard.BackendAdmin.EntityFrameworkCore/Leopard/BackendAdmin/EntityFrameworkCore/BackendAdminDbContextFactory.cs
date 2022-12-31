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
            var configuration = BuildConfiguration();

            string conn = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder<BackendAdminDbContext>()
                .UseMySql(conn, ServerVersion.AutoDetect(conn));

            return new BackendAdminDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Leopard.BackendAdmin.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
