using EShop.Common.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.AuthServer
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<EShopAuthServerIdentityServerModule>(ModuleIdentity.AuthIdentityServer.ServiceType, ModuleIdentity.AuthIdentityServer.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
