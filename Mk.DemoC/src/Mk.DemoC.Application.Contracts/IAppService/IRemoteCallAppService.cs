using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mk.DemoC.IAppService
{
    public interface IRemoteCallAppService: IApplicationService
    {
        Task<string> WelcomeToCAsync();
    }
}
