using Leopard;
using Leopard.Buiness.Shared;
using Leopard.Utils;

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
