using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Mk.DemoC.ElastcSearchAppService;
using Volo.Abp;

namespace Mk.DemoC
{
    [DependsOn(
        typeof(DemoCDomainModule),
        typeof(DemoCApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class DemoCApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<DemoCApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DemoCApplicationModule>(validate: true);
            });


        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            try
            {
                var esClient = context.ServiceProvider.GetService<ElasticSearchClient>();
                esClient.InitIndex();
            }
            catch
            {
                
            }
        }

    }
}
