using EShop.Common.Shared;
using Leopard.Gateway;
using System.Threading.Tasks;

namespace AdministrationGateway.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonGatewayProgram<AdministrationGatewayHostModule>(ModuleIdentity.AdministrationGateway.ServiceType, ModuleIdentity.AdministrationGateway.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
