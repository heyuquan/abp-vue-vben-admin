using EShop.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Identity;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var commonProgram = new CommonHostProgram<EShopIdentityHttpApiHostModule>(ModuleIdentity.Identity.ServiceType, ModuleIdentity.Identity.Name);
        return await commonProgram.RunAsync(args);
    }
}
