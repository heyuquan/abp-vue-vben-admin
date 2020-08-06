using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Leopard.Abp.FeatureManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class LeopardFeatureManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardFeatureManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IFeatureManagementDbContext>();

                options.AddRepository<FeatureValue, EfCoreFeatureValueRepository>();
            });

            context.Services.TryAddTransient(typeof(IFeatureValueRepository), typeof(EfCoreFeatureValueRepository));
        }
    }
}