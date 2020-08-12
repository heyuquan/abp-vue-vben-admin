using Mk.DemoC.IAppService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoC.RemoteCallAppService
{
    public class RemoteCallAppService : DemoCAppService, IRemoteCallAppService
    {
        public RemoteCallAppService()
        {

        }

        public string WelcomeToC()
        {
            return "welcome to c,i am demo c";
        }
    }
}
