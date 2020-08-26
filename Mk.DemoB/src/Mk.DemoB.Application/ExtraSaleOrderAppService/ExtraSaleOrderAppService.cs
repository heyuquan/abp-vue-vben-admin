using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExtraSaleOrderAppService
{
    public class ExtraSaleOrderAppService: DemoBAppService
    {
        private readonly IRepository<SaleOrder, Guid> _saleOrderRepository;

        public ExtraSaleOrderAppService(
            IRepository<SaleOrder, Guid> saleOrderRepository
            )
        {
            _saleOrderRepository = saleOrderRepository;
        }

        ///// <summary>
        ///// 创建订单
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpGet("Paging")]
        //public virtual async Task<ServiceResult<PagedResultDto<SaleOrderDto>>> GetSaleOrderPagingAsync(GetSaleOrderPagingRequest req)
        //{
        //    ServiceResult<PagedResultDto<SaleOrderDto>> ret = new ServiceResult<PagedResultDto<SaleOrderDto>>(IdProvider.Get());

        //    _saleOrderRepository
        //        .WhereIf(req.BeginTime.HasValue,x=>x.order)
        //    return null;
        //}
    }
}
