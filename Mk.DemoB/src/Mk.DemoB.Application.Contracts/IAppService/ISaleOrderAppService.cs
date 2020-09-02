using Leopard.Results;
using Mk.DemoB.Dto.SaleOrders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mk.DemoB.IAppService
{
    public interface ISaleOrderAppService : IApplicationService
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        Task<ServiceResult<SaleOrderDto>> CreateAsync(SaleOrderCreateDto input);

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
       Task<ServiceResult<PagedResultDto<SaleOrderDto>>> GetOrderPagingAsync(GetSaleOrderPagingRequest req);
    }
}
