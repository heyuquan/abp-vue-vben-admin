using Leopard;
using Leopard.Buiness.Shared;
using Leopard.Utils;

namespace InternalGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            GatewayCommonProgram commonProgram = new GatewayCommonProgram(ModuleIdentity.InternalGateway.ServiceType, ModuleIdentity.InternalGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
