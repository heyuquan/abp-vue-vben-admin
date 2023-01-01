using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EShop.Account.Admin
{
    [DependsOn(
        typeof(EShopAccountSharedApplicationContractsModule)
        )]
    public class EShopAccountAdminApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
