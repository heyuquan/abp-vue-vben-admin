using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Leopard.Abp.AuditLogging.EntityFrameworkCore
{
    [DependsOn(typeof(AbpAuditLoggingDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class LeopardAuditLoggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LeopardAuditLoggingDbContext>(options =>
            {
                options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
            });

            context.Services.TryAddTransient(typeof(IAuditLogRepository), typeof(EfCoreAuditLogRepository));
        }
    }
}
