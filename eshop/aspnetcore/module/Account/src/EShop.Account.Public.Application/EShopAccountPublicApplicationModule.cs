using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.UI.Navigation;
using Leopard.Identity;

namespace EShop.Account.Public
{
    [DependsOn( 
        typeof(AbpSmsModule),
        typeof(LeopardIdentityApplicationModule),
        typeof(EShopAccountPublicApplicationContractsModule),
        typeof(EShopAccountSharedApplicationModule)
        )]
    public class EShopAccountPublicApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<EShopAccountPublicApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EShopAccountPublicApplicationModule>(validate: true);
            });
        }
    }
}
