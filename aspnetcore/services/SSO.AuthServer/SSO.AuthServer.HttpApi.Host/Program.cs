using Leopard.Base.Shared;
using Leopard.Host;

namespace SSO.AuthServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ModuleIdentity.Auth.ServiceType, ModuleIdentity.Auth.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
