using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.UI.Navigation;
using Leopard.Identity;

namespace Leopard.Account.Public
{
    [DependsOn( 
        typeof(AbpSmsModule),
        typeof(LeopardIdentityApplicationModule),
        typeof(LeopardAccountPublicApplicationContractsModule),
        typeof(LeopardAccountSharedApplicationModule)
        )]
    public class LeopardAccountPublicApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<LeopardAccountPublicApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LeopardAccountPublicApplicationModule>(validate: true);
            });
        }
    }
}
