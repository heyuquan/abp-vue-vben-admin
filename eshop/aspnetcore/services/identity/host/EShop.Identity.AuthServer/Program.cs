using EShop.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Identity;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var leopardProgram = new LeopardHostProgram<EShopIdentityAuthServerModule>(ModuleIdentity.IdentityAuthServer.ServiceType, ModuleIdentity.IdentityAuthServer.Name);
        return await leopardProgram.RunAsync(args);
    }
}
