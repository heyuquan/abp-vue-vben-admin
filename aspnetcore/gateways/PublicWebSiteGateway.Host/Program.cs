using Leopard;
using Leopard.AspNetCore.Serilog;
using Leopard.Buiness.Shared;
using Leopard.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace PublicWebSiteGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            GatewayCommonProgram commonProgram = new GatewayCommonProgram(ModuleIdentity.PublicWebSiteGateway.ServiceType, ModuleIdentity.PublicWebSiteGateway.Name);
            return commonProgram.CommonMain<Startup>(args);
        }
    }
}
