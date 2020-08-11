using Mk.DemoC.IAppService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Application
{
    public class RemoteCallAppService : DemoBAppService
    {
        private readonly IRemoteCallAppService _remoteCallAppService;
        public RemoteCallAppService(IRemoteCallAppService remoteCallAppService)
        {
            _remoteCallAppService = remoteCallAppService;
        }

        public string CallDemoCAction()
        {
            return _remoteCallAppService.WelcomeToC();
        }
    }
}
