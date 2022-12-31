using Leopard;
using Leopard.Host;
using System.Threading.Tasks;

namespace Mk.DemoC
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<DemoCHttpApiHostModule>(ApplicationServiceType.ApiHost, typeof(Program).Assembly.GetName().Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
