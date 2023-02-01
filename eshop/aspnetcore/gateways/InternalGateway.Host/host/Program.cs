using EShop.Common.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace InternalGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<InternalGatewayHostModule>(ModuleIdentity.InternalGateway.ServiceType, ModuleIdentity.InternalGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
