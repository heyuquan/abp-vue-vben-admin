using Leopard.Base.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace EShop.AuthServer
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<AuthServerHttpApiHostModule>(ModuleIdentity.Auth.ServiceType, ModuleIdentity.Auth.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
