using Leopard.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mk.DemoC.EntityFrameworkCore
{
    [DependsOn(
        typeof(DemoCDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(LeopardEntityFrameworkCoreModule)
    )]
    public class DemoCEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DemoCDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}