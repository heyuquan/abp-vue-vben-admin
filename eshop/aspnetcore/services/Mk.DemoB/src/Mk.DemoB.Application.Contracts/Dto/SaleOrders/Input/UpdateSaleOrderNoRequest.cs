using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Dto.SaleOrders
{
    public class UpdateSaleOrderNoRequest
    {
        public Guid Id { get; set; }

        public string OrderNo { get; set; }
    }
}
