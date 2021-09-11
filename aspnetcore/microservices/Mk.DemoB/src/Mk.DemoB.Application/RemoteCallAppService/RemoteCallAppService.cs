using Microsoft.AspNetCore.Components;
using Mk.DemoC.IAppService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoB.Application
{
    // 远程调用DemoC
    // 1、DemoC 在 Mk.DemoC.Application.Contracts 中要提供 接口 I**AppService
    // 2、DemoB 使用的地方，引用 Mk.DemoC.HttpApi.Client 程序集。并且DemoBModule 要依赖DemoCHttpApiClientModule模块，来创建代理
    // 

    /// <summary>
    /// 动态 C# API 客户端
    /// </summary>
    [Route("api/demob/remote-call")]
    public class RemoteCallAppService : DemoBAppService
    {
        private readonly IRemoteCallAppService _remoteCallAppService;
        public RemoteCallAppService(IRemoteCallAppService remoteCallAppService)
        {
            _remoteCallAppService = remoteCallAppService;
        }

        public async Task<string> CallDemoCAction()
        {
            string ret = await _remoteCallAppService.WelcomeToCAsync();
            return ret;
        }
    }
}
