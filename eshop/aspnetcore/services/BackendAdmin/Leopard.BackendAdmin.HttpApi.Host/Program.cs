using Leopard.Base.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace Leopard.BackendAdmin
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<LeopardBackendAdminHttpApiHostModule>(ModuleIdentity.BackendAdmin.ServiceType, ModuleIdentity.BackendAdmin.Name);
            return await commonProgram.RunAsync(args);
        }

    }
}
