using Mk.DemoB.Enums.SaleOrder;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using System.Linq;

namespace Mk.DemoB.SaleOrderMgr.Entities
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public class SaleOrder : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public SaleOrderStatus OrderStatus { get; set; }

        public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

        public SaleOrder(Guid id, string orderNo, string currency)
        {
            Id = id;
            OrderNo = orderNo;
            Currency = currency;
            TotalAmount = 0;
            OrderStatus = SaleOrderStatus.UnPay;
            SaleOrderDetails = new HashSet<SaleOrderDetail>();

            ExtraProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加产品项
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(SaleOrderDetail item)
        {
            item.LineNo = SaleOrderDetails.Count() + 1;
            item.ParentId = this.Id;
            SaleOrderDetails.Add(item);

            this.TotalAmount += item.Price * item.Quantity;
        }

    }
}
