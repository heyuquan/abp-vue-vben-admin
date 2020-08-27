using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Mk.DemoB.Dto.ExtraSaleOrder;
using Mk.DemoB.Dto.SaleOrders;
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


namespace Mk.DemoB.ExtraSaleOrderAppService
{
    // 如何将 dto 中的 ExtraProperties 字段映射到 实体中 ExtraProperties
    // dto.MapExtraPropertiesTo(role);   只映射单个字段

    [Route("api/demob/extra-sale-order")]
    public class ExtraSaleOrderAppService: DemoBAppService
    {
        private readonly IRepository<SaleOrder, Guid> _saleOrderRepository;

        public ExtraSaleOrderAppService(
            IRepository<SaleOrder, Guid> saleOrderRepository
            )
        {
            _saleOrderRepository = saleOrderRepository;
        }

        /// <summary>
        /// 创建订单
        /// 使用 Abp的对象扩展设置了Customer字段
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public virtual async Task<ServiceResult<SaleOrderDto>> CreateSaleOrderAsync(CreateSaleOrderRequestEx req)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());

            SaleOrder order = new SaleOrder(
                GuidGenerator.Create()
                , $"A0{new Random().Next(100000, 999999)}"
                , req.OrderTime.HasValue ? req.OrderTime.Value : Clock.Now
                , req.Currency
                );

            order.SetProperty("CustomerName", req.CustomerName);
            order.SetProperty("CustomerName2", "我没有独立字段来存储");

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
