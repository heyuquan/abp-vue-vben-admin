using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.SaleOrderAppService
{
    // 关于ABP聚合根类AggregateRoot的思考
    // https://www.cnblogs.com/sheepswallow/p/6272795.html

    // 实体定义
    // SaleOrder：为聚合根，继承 FullAuditedAggregateRoot<Guid> 
    //            聚合根中可发布领域事件
    // SaleOrderDetail：为实体，继承 FullAuditedEntity<Guid>

    // SaleOrder 使用到功能点：本地领域事件、分布式领域事件


    [Route("api/demob/sale-order")]
    public class SaleOrderAppService : DemoBAppService
    {
        private readonly IRepository<SaleOrder, Guid> _saleOrderRepository;

        public SaleOrderAppService(
            IRepository<SaleOrder, Guid> saleOrderRepository
            )
        {
            _saleOrderRepository = saleOrderRepository;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public virtual async Task<ServiceResult<SaleOrderDto>> CreateSaleOrderAsync(CreateSaleOrderRequest req)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());

            SaleOrder order = new SaleOrder(
                GuidGenerator.Create()
                , $"A0{new Random().Next(100000, 999999)}"
                , req.OrderTime.HasValue ? req.OrderTime.Value : Clock.Now
                , req.Currency
                );

            foreach (var item in req.SaleOrderDetails)
            {
                SaleOrderDetail orderDetail = new SaleOrderDetail(
                    GuidGenerator.Create()
                    , order.Id, item.ProductSkuCode, item.Price, item.Quantity
                    );

                order.AddItem(orderDetail);
            }

            await _saleOrderRepository.InsertAsync(order);

            var dto = ObjectMapper.Map<SaleOrder, SaleOrderDto>(order);
            retValue.SetSuccess(dto);
            return retValue;

        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        public virtual async Task<ServiceResult<PagedResultDto<SaleOrderDto>>> GetSaleOrderPagingAsync(GetSaleOrderPagingRequest req)
        {
            ServiceResult<PagedResultDto<SaleOrderDto>> ret = new ServiceResult<PagedResultDto<SaleOrderDto>>(IdProvider.Get());

            var query = _saleOrderRepository
               .WhereIf(req.BeginTime.HasValue, x => x.OrderTime > req.BeginTime.Value)
               .WhereIf(req.EndTime.HasValue, x => x.OrderTime < req.EndTime.Value);

            var count = await query.LongCountAsync();
            var saleOrders=await query.Include(x=>x.SaleOrderDetails).PageBy(req.SkipCount, req.MaxResultCount).ToListAsync();

            List<SaleOrderDto> dtos = ObjectMapper.Map<List<SaleOrder>, List<SaleOrderDto>>(saleOrders);
            var pageData = new PagedResultDto<SaleOrderDto>(count, dtos);
            ret.SetSuccess(pageData);
            return ret;
        }
    }
}
