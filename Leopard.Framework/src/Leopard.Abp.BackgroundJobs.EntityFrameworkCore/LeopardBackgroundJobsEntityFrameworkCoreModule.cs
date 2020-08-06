using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Leopard.Abp.BackgroundJobs.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class LeopardBackgroundJobsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardBackgroundJobsDbContext>(options =>
            {
                 options.AddRepository<BackgroundJobRecord, EfCoreBackgroundJobRepository>();
            });

            context.Services.TryAddTransient(typeof(IBackgroundJobRepository), typeof(EfCoreBackgroundJobRepository));
        }
    }
}