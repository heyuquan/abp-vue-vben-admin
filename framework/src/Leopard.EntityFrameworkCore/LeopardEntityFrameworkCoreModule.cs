using Leopard.EntityFrameworkCore.Logger;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Leopard.EntityFrameworkCore
{
    public class LeopardEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddEFLogger();

            context.Services.Configure<EFLogOptions>(configuration.GetSection("EFCore:EFLog"));
        }
    }
}
