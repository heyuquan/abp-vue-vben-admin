using Mk.DemoB.Domain.Enums.SaleOrders;
using System;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class GetSaleOrderPagingRequest : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 最大总金额
        /// </summary>
        public decimal? MaxTotalAmount { get; set; }
        /// <summary>
        /// 最小总金额
        /// </summary>
        public decimal? MinTotalAmount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public SaleOrderStatus? OrderStatus { get; set; }

    }
}
