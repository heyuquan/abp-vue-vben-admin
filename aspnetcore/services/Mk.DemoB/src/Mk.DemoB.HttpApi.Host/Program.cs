using Leopard;
using Leopard.Host;
using System.Threading.Tasks;

namespace Mk.DemoB
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var commonProgram = new CommonHostProgram<DemoBHttpApiHostModule>(ApplicationServiceType.ApiHost, typeof(Program).Assembly.GetName().Name);
            return await commonProgram.RunAsync(args);
        }
    }
}
