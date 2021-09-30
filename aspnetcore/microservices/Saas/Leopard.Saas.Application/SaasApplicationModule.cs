using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace Leopard.Saas
{
    [DependsOn(
        typeof(SaasDomainModule),
        typeof(SaasApplicationContractsModule),
        typeof(AbpEmailingModule)
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
