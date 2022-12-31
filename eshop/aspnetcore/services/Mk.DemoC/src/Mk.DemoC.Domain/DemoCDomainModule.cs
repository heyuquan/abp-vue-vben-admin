using Volo.Abp.Modularity;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCDomainSharedModule)
        )]
    public class DemoCDomainModule : AbpModule
    {

    }
}
