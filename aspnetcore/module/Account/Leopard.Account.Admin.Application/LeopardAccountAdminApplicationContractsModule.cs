using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Leopard.Account
{
    [DependsOn(
        typeof(LeopardAccountSharedApplicationContractsModule)
        )]
    public class LeopardAccountAdminApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
