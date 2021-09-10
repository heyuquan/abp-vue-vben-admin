using Leopard.AspNetCore.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static int Main(string[] args)
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;

            SerilogConfigurationHelper.Configure(env, assemblyName, true, false);

            try
            {
                Log.Information($"Starting {assemblyName}.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
