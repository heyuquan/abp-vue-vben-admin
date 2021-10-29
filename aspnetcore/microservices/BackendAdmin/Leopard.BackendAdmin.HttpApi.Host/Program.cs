using Leopard.Utils;

namespace Leopard.BackendAdmin
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonProgram commonProgram = new CommonProgram(ApplicationServiceType.ApiHost, typeof(Program).Assembly.GetName().Name);
            return commonProgram.CommonMain<Startup>(args);
        }

    }
}
