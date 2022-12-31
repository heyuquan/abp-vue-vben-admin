using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class DemoBApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            DemoBDtoExtensions.Configure();
        }
    }
}
