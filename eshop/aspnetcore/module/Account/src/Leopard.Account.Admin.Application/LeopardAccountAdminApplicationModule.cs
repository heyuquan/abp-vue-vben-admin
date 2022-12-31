using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Leopard.Account.Admin
{
    [DependsOn(
        typeof(LeopardAccountSharedApplicationModule),
        typeof(LeopardAccountAdminApplicationContractsModule)
        )]
    public class LeopardAccountAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<LeopardAccountAdminApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LeopardAccountAdminApplicationModule>(validate: true);
            });
        }
    }
}
