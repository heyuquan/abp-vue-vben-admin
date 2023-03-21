using EShop.Common.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace AdministrationAppGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<AdministrationAppGatewayHostModule>(ModuleIdentity.AdministrationGateway.ServiceType, ModuleIdentity.AdministrationGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
