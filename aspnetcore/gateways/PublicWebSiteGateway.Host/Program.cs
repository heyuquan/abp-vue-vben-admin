using Leopard.Base.Shared;
using Leopard.Gateway;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CommonGatewayProgram commonProgram = new CommonGatewayProgram(ModuleIdentity.PublicWebSiteGateway.ServiceType, ModuleIdentity.PublicWebSiteGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
