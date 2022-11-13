using Leopard.Base.Shared;
using Leopard.Host;
using System.Threading.Tasks;

namespace SSO.AuthServer
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<AuthServerIdentityServerModule>(ModuleIdentity.AuthIdentityServer.ServiceType, ModuleIdentity.AuthIdentityServer.Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
