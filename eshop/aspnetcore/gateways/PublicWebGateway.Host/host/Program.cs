using EShop.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<PublicWebGatewayHostModule>(ModuleIdentity.PublicWebGateway.ServiceType, ModuleIdentity.PublicWebGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
