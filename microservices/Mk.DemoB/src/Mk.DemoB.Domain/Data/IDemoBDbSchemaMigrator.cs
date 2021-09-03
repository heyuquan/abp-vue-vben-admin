using System.Threading.Tasks;

namespace Mk.DemoB.Data
{
    public interface IDemoBDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
