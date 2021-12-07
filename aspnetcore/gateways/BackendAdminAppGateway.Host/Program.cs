using Leopard;
using Leopard.Buiness.Shared;
using Leopard.Utils;

namespace BackendAdminAppGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            GatewayCommonProgram commonProgram = new GatewayCommonProgram(ModuleIdentity.BackendAdminAppGateway.ServiceType, ModuleIdentity.BackendAdminAppGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
