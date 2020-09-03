using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.SaleOrderMgr;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using System.Linq;
using Volo.Abp;
using Mk.DemoB.IAppService;
using Volo.Abp.ObjectExtending;
using Volo.Abp.EventBus.Local;
using Mk.DemoB.SaleOrderMgr.Events;

namespace Mk.DemoB.SaleOrderAppService
{
    // 关于ABP聚合根类AggregateRoot的思考
    // https://www.cnblogs.com/sheepswallow/p/6272795.html

    // 实体定义
    // SaleOrder：为聚合根，继承 FullAuditedAggregateRoot<Guid> 
    //            聚合根中可发布领域事件
    // SaleOrderDetail：为实体，继承 FullAuditedEntity<Guid>

    // SaleOrder 聚合根使用到功能点：本地领域事件、分布式领域事件

    // 领域事件：
    // 再应用层直接注入ILocalEventBus，在调用PublishAsync就直接发布事件了。
    // 在领域根里面使用领域根的发布事件，则是在SaveChange成功后才发布事件

    [Route("api/demob/sale-order")]
    public class SaleOrderAppService : DemoBAppService, ISaleOrderAppService
    {
        private readonly ISaleOrderRepository _saleOrderRepository;

        public SaleOrderAppService(
            ISaleOrderRepository saleOrderRepository
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
        public virtual async Task<ServiceResult<SaleOrderDto>> CreateAsync(SaleOrderCreateDto input)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());

            SaleOrder saleOrder = new SaleOrder(
                GuidGenerator.Create()
                , CurrentTenant.Id
                , input.OrderNo
                , input.OrderTime
                , input.Currency
                );

            input.MapExtraPropertiesTo(saleOrder);

            foreach (var item in input.SaleOrderDetails)
            {
                SaleOrderDetail orderDetail = new SaleOrderDetail(
                    GuidGenerator.Create(), CurrentTenant.Id
                    , saleOrder.Id, item.ProductSkuCode, item.Price, item.Quantity
                    );

                saleOrder.AddItem(orderDetail);
            }

            saleOrder.SumDetail();
            await _saleOrderRepository.InsertAsync(saleOrder);

            var dto = ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder);
            retValue.SetSuccess(dto);
            return retValue;

        }

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        public virtual async Task<ServiceResult<PagedResultDto<SaleOrderDto>>> GetOrderPagingAsync(GetSaleOrderPagingRequest req)
        {
            ServiceResult<PagedResultDto<SaleOrderDto>> retValue = new ServiceResult<PagedResultDto<SaleOrderDto>>(IdProvider.Get());

            var pageData = await _saleOrderRepository.GetPagingAsync(
                            req.OrderNo
                            , req.OrderStatus
                            , req.BeginTime, req.EndTime
                            , req.MaxTotalAmount, req.MinTotalAmount
                            , req.Sorting
                            , req.SkipCount, req.MaxResultCount
                            );

            List<SaleOrderDto> dtos = ObjectMapper.Map<List<SaleOrder>, List<SaleOrderDto>>(pageData.Items);
            var pageResult = new PagedResultDto<SaleOrderDto>(pageData.TotalCount, dtos);
            retValue.SetSuccess(pageResult);
            return retValue;
        }

        /// <summary>
        /// 根据Id查询订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("id/{id}")]
        public virtual async Task<ServiceResult<SaleOrderDto>> GetByIdAsync(Guid id)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());
            var saleOrder = await _saleOrderRepository.FindAsync(id);

            retValue.SetSuccess(ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder));
            return retValue;
        }

        /// <summary>
        /// 根据 订单编号 查询订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("orderno/{orderNo}")]
        public virtual async Task<ServiceResult<SaleOrderDto>> GetByOrderNoAsync(string orderNo)
        {
            Check.NotNullOrEmpty(orderNo, nameof(orderNo));

            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());
            var saleOrder = await _saleOrderRepository.GetByOrderNoAsync(orderNo);

            retValue.SetSuccess(ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder));
            return retValue;
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public virtual async Task<ServiceResult<SaleOrderDto>> UpdateAsync(SaleOrderUpdateDto input)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());
            var saleOrder = await _saleOrderRepository.FindAsync(input.Id);

            // ConcurrencyStamp 并发检查，发生并发会抛出异常：AbpDbConcurrencyException
            saleOrder.ConcurrencyStamp = input.ConcurrencyStamp;
            saleOrder.OrderNo = input.OrderNo;
            saleOrder.OrderTime = input.OrderTime;
            saleOrder.Currency = input.Currency;

            input.MapExtraPropertiesTo(saleOrder);

            foreach (var item in input.SaleOrderDetails)
            {
                if (item.Id.HasValue)
                {
                    var subItem = saleOrder.SaleOrderDetails.Where(x => x.Id == item.Id.Value).FirstOrDefault();
                    if (subItem == null)
                    {
                        throw new UserFriendlyException($"不存在ID为[{item.Id.Value}]的子表数据");
                    }

                    if (item.IsDelete)
                    {
                        // 删除
                        saleOrder.DeleteItem(subItem);
                    }
                    else
                    {
                        subItem.Price = item.Price;
                        // 修改
                        if (string.Compare(subItem.ProductSkuCode, item.ProductSkuCode) == 0)
                        {                            
                            if (subItem.Quantity != item.Quantity)
                            {
                                saleOrder.ChangeSkuQuantity(subItem, item.Quantity);
                            }
                        }
                        else
                        {
                            saleOrder.ChangeSku(subItem, item.ProductSkuCode, item.Quantity);
                        }
                    }
                }
                else
                {
                    Guid orderItemId = GuidGenerator.Create();
                    // 新增
                    SaleOrderDetail orderItem = new SaleOrderDetail(
                        orderItemId, CurrentTenant.Id
                        , saleOrder.Id, item.ProductSkuCode, item.Price, item.Quantity
                        );

                    saleOrder.AddItem(orderItem);
                }
            }

            saleOrder.SumDetail();
            await _saleOrderRepository.UpdateAsync(saleOrder);
            var dto = ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder);
            retValue.SetSuccess(dto);
            return retValue;
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<ServiceResult> DeleteByIdAsync(Guid id)
        {
            ServiceResult<SaleOrderDto> retValue = new ServiceResult<SaleOrderDto>(IdProvider.Get());

            // 直接通过 id 删除实体，并不会把关联的子表一起删除。所以需要将实体和实体的子表查出来，再删除
            //await _saleOrderRepository.DeleteAsync(id);
            var saleOrder = await _saleOrderRepository.FindAsync(id);

            await _saleOrderRepository.DeleteAsync(saleOrder);
            retValue.SetSuccess();
            return retValue;
        }
    }
}
