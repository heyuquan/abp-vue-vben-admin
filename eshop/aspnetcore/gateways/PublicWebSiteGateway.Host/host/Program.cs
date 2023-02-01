using EShop.Common.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<PublicWebSiteGatewayHostModule>(ModuleIdentity.PublicWebSiteGateway.ServiceType, ModuleIdentity.PublicWebSiteGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
