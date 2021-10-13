using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Leopard.BackendAdmin.Data
{
    /* This is used if database provider does't define
     * IBackendAdminDbSchemaMigrator implementation.
     */
    public class NullBackendAdminDbSchemaMigrator : IBackendAdminDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}