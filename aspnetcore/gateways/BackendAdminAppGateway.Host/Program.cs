using Leopard.Buiness.Shared;
using Leopard.Gateway;

namespace BackendAdminAppGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonGatewayProgram commonProgram = new CommonGatewayProgram(ModuleIdentity.BackendAdminAppGateway.ServiceType, ModuleIdentity.BackendAdminAppGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
