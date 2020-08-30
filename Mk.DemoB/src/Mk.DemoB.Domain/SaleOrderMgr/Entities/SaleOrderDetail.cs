using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Mk.DemoB.SaleOrderMgr.Entities
{
    /// <summary>
    /// 销售订单详情
    /// </summary>
    public class SaleOrderDetail : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        /// <summary>
        /// 父表ID
        /// </summary>
        public Guid ParentId { get; protected set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// 产品Sku编号
        /// </summary>
        public string ProductSkuCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        public SaleOrderDetail(Guid id, Guid? tenantId, Guid parentId
            , string productSkuCode, decimal price, int quantity)
        {
            Id = id;
            TenantId = tenantId;
            ParentId = parentId;
            ProductSkuCode = productSkuCode;
            Price = price;
            Quantity = quantity;
        }

    }
}
