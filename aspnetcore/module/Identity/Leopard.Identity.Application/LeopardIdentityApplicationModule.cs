using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace Leopard.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),        
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(LeopardIdentityApplicationContractsModule)
        )]
    public class LeopardIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<LeopardIdentityApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LeopardIdentityApplicationModule>(validate: true);
            });
        }
    }
}
