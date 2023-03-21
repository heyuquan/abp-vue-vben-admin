using EShop.Common.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Identity;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var commonProgram = new CommonHostProgram<EShopIdentityAuthServerModule>(ModuleIdentity.IdentityAuth.ServiceType, ModuleIdentity.IdentityAuth.Name);
        return await commonProgram.RunAsync(args);
    }
}
