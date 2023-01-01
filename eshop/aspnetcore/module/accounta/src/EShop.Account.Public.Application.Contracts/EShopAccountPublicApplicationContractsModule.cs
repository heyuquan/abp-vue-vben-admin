using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Leopard.Identity;

namespace EShop.Account.Public
{
    [DependsOn(
        typeof(EShopAccountSharedApplicationContractsModule)
        )]
    public class EShopAccountPublicApplicationContractsModule : AbpModule
    {

    }
}
