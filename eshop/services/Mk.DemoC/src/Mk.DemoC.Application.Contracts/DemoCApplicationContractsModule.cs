using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class DemoCApplicationContractsModule : AbpModule
    {

    }
}
