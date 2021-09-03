using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoB.Data
{
    /* This is used if database provider does't define
     * IDemoBDbSchemaMigrator implementation.
     */
    public class NullDemoBDbSchemaMigrator : IDemoBDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}