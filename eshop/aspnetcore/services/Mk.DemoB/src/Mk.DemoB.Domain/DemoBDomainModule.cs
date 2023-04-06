using Mk.DemoB.ObjectExtending;
using EShop.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Leopard.Saas;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBDomainSharedModule),
        typeof(LeopardSaasDomainModule)
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
