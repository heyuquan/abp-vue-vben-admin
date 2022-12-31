using Mk.DemoC;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace Mk.DemoB
{
    [DependsOn(
        typeof(DemoBDomainModule),
        typeof(DemoBApplicationContractsModule),
        typeof(AbpFluentValidationModule), 

        // 远程调用C，需要依赖远程  C代理Module
        typeof(DemoCHttpApiClientModule)
        )]
    public class DemoBApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DemoBApplicationModule>();
            });
        }
    }
}
