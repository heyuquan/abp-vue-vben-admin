using Leopard;
using Leopard.Crypto;
using Leopard.Helpers;
using Leopard.Utils;
using System;

namespace Mk.DemoB
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
