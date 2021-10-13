using System.Threading.Tasks;

namespace Leopard.BackendAdmin.Data
{
    public interface IBackendAdminDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
