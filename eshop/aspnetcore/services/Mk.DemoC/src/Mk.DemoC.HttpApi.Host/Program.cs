using Leopard;
using Leopard.Host;
using System.Threading.Tasks;

namespace Mk.DemoC
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardHostProgram<DemoCHttpApiHostModule>(ApplicationServiceType.ApiHost, typeof(Program).Assembly.GetName().Name);
            return await leopardProgram.RunAsync(args);
        }
    }
}
