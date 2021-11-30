using Leopard;
using Leopard.Utils;

namespace SSO.AuthServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ApplicationServiceType.AuthIdentityServer, typeof(Program).Assembly.GetName().Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
