using Mk.DemoB.Domain.Enums.SaleOrders;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using System.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Data;
using Mk.DemoB.Domain.Events.SaleOrders;

namespace Mk.DemoB.SaleOrderMgr.Entities
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public class SaleOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
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
        /// 订单状态
        /// </summary>
        public SaleOrderStatus OrderStatus { get; set; }

        public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

        public SaleOrder(Guid id, Guid? tenantId, string orderNo, DateTime orderTime, string currency)
        {
            Id = id;
            TenantId = tenantId;
            OrderNo = orderNo;
            OrderTime = orderTime;
            Currency = currency;
            TotalAmount = 0;
            OrderStatus = SaleOrderStatus.UnPay;
            SaleOrderDetails = new HashSet<SaleOrderDetail>();

            ExtraProperties = new ExtraPropertyDictionary();
        }

        /// <summary>
        /// 添加产品项
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(SaleOrderDetail item)
        {
            item.LineNo = SaleOrderDetails.Count() + 1;
            SaleOrderDetails.Add(item);

            AddLocalEvent(new SaleOrderSkuQuantityChangedEvent
            {
                SaleOrderId = Id,
                SaleOrderDetailId = item.Id,
                ProductSkuCode = item.ProductSkuCode,
                ChangeQuantity = item.Quantity
            });

            this.TotalAmount += item.Price * item.Quantity;
        }

        /// <summary>
        /// 删除产品项
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItem(SaleOrderDetail item)
        {
            item.LineNo = SaleOrderDetails.Count() + 1;
            SaleOrderDetails.Remove(item);

            AddLocalEvent(new SaleOrderSkuQuantityChangedEvent
            {
                SaleOrderId = Id,
                SaleOrderDetailId = item.Id,
                ProductSkuCode = item.ProductSkuCode,
                ChangeQuantity = -item.Quantity
            });

            this.TotalAmount += item.Price * item.Quantity;
        }

        /// <summary>
        /// 修改Sku数量
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newQuantity"></param>
        public void ChangeSkuQuantity(SaleOrderDetail item, int newQuantity)
        {
            AddLocalEvent(new SaleOrderSkuQuantityChangedEvent
            {
                SaleOrderId = Id,
                SaleOrderDetailId = item.Id,
                ProductSkuCode = item.ProductSkuCode,
                ChangeQuantity = newQuantity - item.Quantity
            });

            item.Quantity = newQuantity;
        }

        /// <summary>
        /// 改变SkuCode
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newQuantity"></param>
        public void ChangeSku(SaleOrderDetail item, string productSkuCode, int quantity)
        {
            // 发布旧sku的变更事件
            AddLocalEvent(new SaleOrderSkuQuantityChangedEvent
            {
                SaleOrderId = Id,
                SaleOrderDetailId = item.Id,
                ProductSkuCode = item.ProductSkuCode,
                ChangeQuantity = -item.Quantity
            });

            item.ProductSkuCode = productSkuCode;
            item.Quantity = quantity;

            // 发布新sku的变更事件
            AddLocalEvent(new SaleOrderSkuQuantityChangedEvent
            {
                SaleOrderId = Id,
                SaleOrderDetailId = item.Id,
                ProductSkuCode = item.ProductSkuCode,
                ChangeQuantity = item.Quantity
            });
        }

        /// <summary>
        /// 汇总子表数据
        /// </summary>
        public void SumDetail()
        {
            decimal totalAmount = 0;
            foreach (var item in SaleOrderDetails)
            {
                totalAmount += item.Price * item.Quantity;
            }
            TotalAmount = totalAmount;
        }

        /// <summary>
        /// 发布销售订单创建成功事件
        /// </summary>
        public void PublishCreateSuccessEvent()
        {
            string customerName = this.GetProperty<string>("CustomerName");
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                AddDistributedEvent(new SaleOrderCreatedEvent
                {
                    CustomerName = customerName
                });
            }
        }
    }
}
