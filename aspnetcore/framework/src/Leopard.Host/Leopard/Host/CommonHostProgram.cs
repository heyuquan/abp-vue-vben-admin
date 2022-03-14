using Leopard.AspNetCore.Serilog;
using Leopard.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Leopard.Host
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class CommonProgram
    {
        /// <summary>
        /// 模块名（模块key）eg：Leopard.Saas
        /// </summary>
        protected string ModuleKey { get; private set; }
        protected ApplicationServiceType ApplicationServiceType { get; private set; }

        public CommonProgram(ApplicationServiceType serviceType, string moduleKey)
        {
            ApplicationServiceType = serviceType;
            ModuleKey = moduleKey;
        }

        private static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public int CommonMain<T>(string[] args) where T : class
        {
            SerilogConfigurationHelper.Configure(env, ModuleKey, true, false);

            try
            {
                Log.Information($"Starting {ModuleKey}.");
                var host = CreateHostBuilder<T>(args).Build();

                // 如何实现 asp.net core 安全优雅退出 ?
                // https://mp.weixin.qq.com/s/TwPNPwD-XlmKiuuYYk1MeQ
                var life = host.Services.GetRequiredService<IHostApplicationLifetime>();
                life.ApplicationStopped.Register(() =>
                {
                    Console.WriteLine($"{ModuleKey} is shut down");
                });


                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{ModuleKey} terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal IHostBuilder CreateHostBuilder<T>(string[] args) where T : class =>
           Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    this.ConfigureAppConfiguration(hostingContext, config);
                    config.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<T>();
                })
                .UseAutofac()
                .UseSerilog();

        protected virtual void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            // 给子类重写用
        }
    }
}
