using Leopard;
using Leopard.Utils;

namespace InternalGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ApplicationServiceType.GateWay, typeof(Program).Assembly.GetName().Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
