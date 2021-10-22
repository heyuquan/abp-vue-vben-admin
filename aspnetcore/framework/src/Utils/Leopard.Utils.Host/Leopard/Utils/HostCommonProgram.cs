using Leopard.AspNetCore.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Leopard.Utils
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class CommonProgram
    {
        private  string _assemblyName { get; set; }
        

        public CommonProgram(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public int CommonMain<T>(string[] args) where T : class
        {
            SerilogConfigurationHelper.Configure(env, _assemblyName, true, false);

            try
            {
                Log.Information($"Starting {_assemblyName}.");
                CreateHostBuilder<T>(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{_assemblyName} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal IHostBuilder CreateHostBuilder<T>(string[] args) where T : class =>
           Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(build =>
                {
                    // 先加载公共
                    build.AddJsonFile("AppConfig/commsettings.json", optional: true);
                    // 再加载独立的
                    build.AddJsonFile("appsettings.secrets.json", optional: true);                    
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<T>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
