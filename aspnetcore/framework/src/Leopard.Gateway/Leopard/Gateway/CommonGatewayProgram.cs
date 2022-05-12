using Leopard.Helpers.IO;
using Leopard.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace Leopard.Gateway
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class CommonGatewayProgram : CommonProgram
    {
        public CommonGatewayProgram(ApplicationServiceType serviceType, string assemblyName) : base(serviceType, assemblyName)
        {
        }

        protected override void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            base.ConfigureAppConfiguration(hostingContext, config);
            config.AddOcelot(
                DirectoryHelper.GetRuntimeDirectory($"./Ocelot_{hostingContext.HostingEnvironment?.EnvironmentName}")
                , hostingContext.HostingEnvironment?.EnvironmentName);
        }
    }
}
