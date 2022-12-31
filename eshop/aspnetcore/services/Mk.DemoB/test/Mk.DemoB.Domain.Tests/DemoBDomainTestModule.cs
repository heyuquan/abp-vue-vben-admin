using Mk.DemoB.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBEntityFrameworkCoreTestModule)
        )]
    public class DemoBDomainTestModule : AbpModule
    {

    }
}