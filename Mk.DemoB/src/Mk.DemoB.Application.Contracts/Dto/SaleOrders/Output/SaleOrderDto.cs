using Mk.DemoB.Enums.SaleOrder;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class SaleOrderDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public SaleOrderStatus OrderStatus { get; set; }

        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<SaleOrderDetailDto> SaleOrderDetails { get; set; }
    }
}
