using Microsoft.AspNetCore.Authorization;
using Mk.DemoC.IAppService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoC.RemoteCallAppService
{
    /// <summary>
    /// 演示其他服务，eg：DemoB 中调用此服务
    /// </summary>
    public class RemoteCallAppService : DemoCAppService, IRemoteCallAppService
    {
        public RemoteCallAppService()
        {

        }

        //[AllowAnonymous]
        public async Task<string> WelcomeToCAsync()
        {
            return await Task.FromResult("welcome to c,i am demo c");
        }
    }
}
