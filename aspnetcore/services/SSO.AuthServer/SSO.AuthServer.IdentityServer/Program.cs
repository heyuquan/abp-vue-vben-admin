using Leopard.Buiness.Shared;
using Leopard.Host;

namespace SSO.AuthServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ModuleIdentity.AuthIdentityServer.ServiceType, ModuleIdentity.AuthIdentityServer.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
