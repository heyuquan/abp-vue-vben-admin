using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace Mk.DemoC.IAppService
{
    public interface IRemoteCallAppService: IApplicationService
    {
        public string WelcomeToC();
    }
}
