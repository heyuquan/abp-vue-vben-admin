using Volo.Abp.Modularity;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBApplicationModule),
        typeof(DemoBDomainTestModule)
        )]
    public class DemoBApplicationTestModule : AbpModule
    {

    }
}