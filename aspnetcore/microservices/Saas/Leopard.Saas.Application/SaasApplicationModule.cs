using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(SaasDomainModule),
        typeof(SaasApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class SaasApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SaasApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SaasApplicationModule>(validate: true);
            });
        }
    }
}
