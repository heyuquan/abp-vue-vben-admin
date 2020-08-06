using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Leopard.Abp.TenantManagement.EntityFrameworkCore
{
    [DependsOn(typeof(AbpTenantManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class LeopardTenantManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardTenantManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementDbContext>();

                options.AddRepository<Tenant, EfCoreTenantRepository>();
            });

            context.Services.TryAddTransient(typeof(ITenantRepository), typeof(EfCoreTenantRepository));
        }
    }
}
