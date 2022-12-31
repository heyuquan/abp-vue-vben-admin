using Leopard.Base.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace BackendAdminAppGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<BackendAdminAppGatewayHostModule>(ModuleIdentity.BackendAdminAppGateway.ServiceType, ModuleIdentity.BackendAdminAppGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
