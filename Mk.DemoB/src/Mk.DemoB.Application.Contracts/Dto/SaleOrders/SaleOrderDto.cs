using Mk.DemoB.Enums.SaleOrder;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderDto : ExtensibleCreationAuditedEntityDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; protected set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; protected set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; protected set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public SaleOrderStatus OrderStatus { get; set; }

        public virtual ICollection<SaleOrderDetailDto> SaleOrderDetails { get; set; }
    }
}
