using Leopard.Buiness.Shared;
using Leopard.Host;

namespace Leopard.BackendAdmin
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ModuleIdentity.BackendAdmin.ServiceType, ModuleIdentity.BackendAdmin.Name);
            return commonProgram.CommonMain<Startup>(args);
        }

    }
}
