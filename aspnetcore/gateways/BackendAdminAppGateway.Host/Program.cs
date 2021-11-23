using Leopard;
using Leopard.Utils;

namespace BackendAdminAppGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            GatewayCommonProgram commonProgram = new GatewayCommonProgram(ApplicationServiceType.GateWay, typeof(Program).Assembly.GetName().Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
