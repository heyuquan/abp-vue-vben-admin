using EShop.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace AdministrationGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardGatewayProgram<AdministrationGatewayHostModule>(ModuleIdentity.AdministrationGateway.ServiceType, ModuleIdentity.AdministrationGateway.Name);
            return await leopardProgram.RunAsync(args);
        }
    }
}
