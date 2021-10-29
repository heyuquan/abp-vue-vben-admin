using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(LeopardSaasDomainModule),
        typeof(LeopardSaasApplicationContractsModule),
        typeof(AbpEmailingModule)
        )]
    public class LeopardSaasApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<LeopardSaasApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LeopardSaasApplicationModule>(validate: true);
            });
        }
    }
}
