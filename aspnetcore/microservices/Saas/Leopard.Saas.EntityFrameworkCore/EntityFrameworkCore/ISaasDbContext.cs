using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Leopard.Saas.EntityFrameworkCore
{
    [ConnectionStringName(SaasDbProperties.ConnectionStringName)]
    public interface ISaasDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}