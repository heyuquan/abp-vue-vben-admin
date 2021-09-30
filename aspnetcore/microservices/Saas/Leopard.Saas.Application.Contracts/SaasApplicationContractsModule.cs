using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(SaasDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class SaasApplicationContractsModule : AbpModule
    {

    }
}
