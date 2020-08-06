using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Leopard.Abp.SettingManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class LeopardSettingManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddAbpDbContext<LeopardSettingManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ISettingManagementDbContext>();

                options.AddRepository<Setting, EfCoreSettingRepository>();
            });

            context.Services.TryAddTransient(typeof(ISettingRepository), typeof(EfCoreSettingRepository));
        }
    }
}
