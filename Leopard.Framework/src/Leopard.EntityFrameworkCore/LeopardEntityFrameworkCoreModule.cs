using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Leopard.EntityFrameworkCore
{
    public class LeopardEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEFLogger();
        }
    }
}
