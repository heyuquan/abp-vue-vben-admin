using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Leopard.Abp.PermissionManagement.EntityFrameworkCore
{
    [DependsOn(typeof(AbpPermissionManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class LeopardPermissionManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardPermissionManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementDbContext>();

                options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
            });

            context.Services.TryAddTransient(typeof(IPermissionGrantRepository), typeof(EfCorePermissionGrantRepository));
        }
    }
}
