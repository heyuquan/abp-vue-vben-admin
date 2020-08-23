using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
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


    [Route("api/SaleOrder")]
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
        [HttpPost("Create")]
        public virtual async Task<ServiceResult<SaleOrderDto>> Create(CreateOrderRequest req)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());

            SaleOrder order = new SaleOrder(
                GuidGenerator.Create()
                , $"A0{new Random().Next(100000, 999999)}"
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

    }
}
