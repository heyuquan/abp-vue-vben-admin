using EShop.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace InternalGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardGatewayProgram<InternalGatewayHostModule>(ModuleIdentity.InternalGateway.ServiceType, ModuleIdentity.InternalGateway.Name);
            return await leopardProgram.RunAsync(args);
        }
    }
}
