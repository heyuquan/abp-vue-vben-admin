using EShop.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Administration
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardHostProgram<EShopAdministrationHttpApiHostModule>(ModuleIdentity.Administration.ServiceType, ModuleIdentity.Administration.Name);
            return await leopardProgram.RunAsync(args);
        }

    }
}
