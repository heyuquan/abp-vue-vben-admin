using Mk.DemoB.ObjectExtending;
using Leopard.Buiness.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBDomainSharedModule)
        )]
    public class DemoBDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            DemoBDomainObjectExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }
    }
}
