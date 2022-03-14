using Leopard.Buiness.Shared;
using Leopard.Gateway;

namespace InternalGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonGatewayProgram commonProgram = new CommonGatewayProgram(ModuleIdentity.InternalGateway.ServiceType, ModuleIdentity.InternalGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
