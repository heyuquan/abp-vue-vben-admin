using EShop.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Identity;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var leopardProgram = new LeopardHostProgram<EShopIdentityHttpApiHostModule>(ModuleIdentity.Identity.ServiceType, ModuleIdentity.Identity.Name);
        return await leopardProgram.RunAsync(args);
    }
}
