using Leopard.Base.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.Administration
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<AdministrationHttpApiHostModule>(ModuleIdentity.Administration.ServiceType, ModuleIdentity.Administration.Name);
            return await commonProgram.RunAsync(args);
        }

    }
}
