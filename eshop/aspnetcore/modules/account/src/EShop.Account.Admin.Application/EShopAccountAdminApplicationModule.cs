using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.SettingManagement;

namespace EShop.Account.Admin
{
    [DependsOn(
        typeof(EShopAccountSharedApplicationModule),
        typeof(EShopAccountAdminApplicationContractsModule),
        typeof(AbpSettingManagementDomainModule)
        )]
    public class EShopAccountAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<EShopAccountAdminApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EShopAccountAdminApplicationModule>(validate: true);
            });
        }
    }
}
