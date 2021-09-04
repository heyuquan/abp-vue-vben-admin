using System.Threading.Tasks;

namespace SSO.AuthServer.Data
{
    public interface IAuthServerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
