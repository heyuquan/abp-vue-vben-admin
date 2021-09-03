﻿using Mk.DemoB.Dto.Guids;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mk.DemoB.IAppService
{
    public interface IGuidAppService: IApplicationService
    {
        Task<string> New();

        /// <summary>
        /// 检测获取的是不是顺序Id
        /// </summary>
        /// <returns></returns>
        Task<SequentialIdsDto> SequentialIds();
    }
}
