using EShop.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardGatewayProgram<PublicWebGatewayHostModule>(ModuleIdentity.PublicWebGateway.ServiceType, ModuleIdentity.PublicWebGateway.Name);
            return await leopardProgram.RunAsync(args);
        }
    }
}
