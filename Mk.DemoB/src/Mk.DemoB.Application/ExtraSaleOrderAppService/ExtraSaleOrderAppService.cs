using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Mk.DemoB.Dto.ExtraSaleOrder;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.IAppService;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Mk.DemoB.ExtraSaleOrderAppService
{
    // 如何将 dto 中的 ExtraProperties 字段映射到 实体中 ExtraProperties
    // dto.MapExtraPropertiesTo(role);   只映射单个字段

    [Route("api/demob/extra-sale-order")]
    public class ExtraSaleOrderAppService : DemoBAppService
    {
        private readonly IRepository<SaleOrder, Guid> _saleOrderRepository;
        private readonly ISaleOrderAppService _saleOrderAppService;

        public ExtraSaleOrderAppService(
            ISaleOrderAppService saleOrderAppService
            , IRepository<SaleOrder, Guid> saleOrderRepository
            )
        {
            _saleOrderRepository = saleOrderRepository;
            _saleOrderAppService = saleOrderAppService;
        }

        /// <summary>
        /// 创建订单
        /// 使用 Abp的对象扩展设置了Customer字段
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [UnitOfWork]
        public virtual async Task<ServiceResult<SaleOrderDto>> CreateAsync(ExtraSaleOrderCreateDto input)
        {
            SaleOrderCreateDto saleOrderCreateDto = input;
            saleOrderCreateDto.SetProperty("CustomerName", input.CustomerName);
            saleOrderCreateDto.SetProperty("CustomerName2", "我没有独立字段来存储");

            return await _saleOrderAppService.CreateAsync(saleOrderCreateDto);
        }
    }
}
