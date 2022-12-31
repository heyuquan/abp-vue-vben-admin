using Leopard.Base.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace AdministrationAppGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<AdministrationAppGatewayHostModule>(ModuleIdentity.AdministrationAppGateway.ServiceType, ModuleIdentity.AdministrationAppGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
