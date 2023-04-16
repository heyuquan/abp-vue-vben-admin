using Leopard.Host;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace Leopard.Gateway
{
    // 备注：为什么不做成 Program 的基类。因为Program中必须要求显示定义 public static int Main(string[] args) 方法
    /// <summary>
    /// 共用 Program
    /// </summary>
    public class LeopardGatewayProgram<TModule> : LeopardHostProgram<TModule>
        where TModule : AbpModule
    {
        public LeopardGatewayProgram(ApplicationServiceType serviceType, string assemblyName) : base(serviceType, assemblyName)
        {
        }

        protected override void ConfigureHostBuilder(IHostBuilder hostBuilder)
        {
            hostBuilder.AddYarpJson();
        }
    }
}
