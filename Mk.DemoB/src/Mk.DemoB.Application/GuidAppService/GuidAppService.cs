﻿using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.Dto;
using Mk.DemoB.IAppService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace Mk.DemoB.Application
{
    public class GuidAppService : DemoBAppService, IGuidAppService
    {
        private readonly IGuidGenerator _guidGenerator;

        public GuidAppService(IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 获取新的GuidId
        /// </summary>
        /// <returns>GuidId</returns>
        [HttpGet]
        public async Task<string> New()
        {
            return await Task.FromResult(_guidGenerator.Create().ToString());
        }

        /// <summary>
        /// 检测获取的是不是顺序Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<SequentialIdsDto> SequentialIds()
        {
            List<string> ids = new List<string>();
            SortedSet<string> sortIds = new SortedSet<string>();

            for (int i = 1; i <= 5; i++)
            {
                string id = _guidGenerator.Create().ToString();
                ids.Add(id);
            }

            sortIds.Add(ids[3]);
            sortIds.Add(ids[0]);
            sortIds.Add(ids[4]);
            sortIds.Add(ids[1]);
            sortIds.Add(ids[2]);

            SequentialIdsDto dto = new SequentialIdsDto()
            {
                Ids = ids,
                SortIds = sortIds
            };

            return await Task.FromResult(dto);
        }
    }
}
