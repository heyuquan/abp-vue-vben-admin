using Leopard;
using Leopard.Host;
using System.Threading.Tasks;

namespace Mk.DemoB
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var leopardProgram = new LeopardHostProgram<DemoBHttpApiHostModule>(ApplicationServiceType.ApiHost, typeof(Program).Assembly.GetName().Name);
            return await leopardProgram.RunAsync(args);
        }
    }
}
