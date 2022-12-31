using Microsoft.EntityFrameworkCore;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mk.DemoB.EntityFrameworkCore
{
    public static class DemoBEfCoreQueryableExtensions
    {
        public static IQueryable<SaleOrder> IncludeDetails(this IQueryable<SaleOrder> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable.Include(x => x.SaleOrderDetails);
        }
    }
}
