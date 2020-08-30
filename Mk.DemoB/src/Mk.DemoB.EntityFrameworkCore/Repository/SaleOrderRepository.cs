using Leopard.Paging;
using Microsoft.EntityFrameworkCore;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.EntityFrameworkCore;
using Mk.DemoB.Enums.SaleOrder;
using Mk.DemoB.SaleOrderMgr;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoB.Repository
{
    public class SaleOrderRepository : EfCoreRepository<DemoBDbContext, SaleOrder, Guid>, ISaleOrderRepository
    {
        public SaleOrderRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// 根据订单编号获取订单
        /// </summary>
        /// <returns></returns>
        public async Task<SaleOrder> GetByOrderNoAsync(string orderNo)
        {
            return await Task.FromResult(DbSet.Where(x => x.OrderNo == orderNo).FirstOrDefault());
        }

        /// <summary>
        /// 查询订单分页数据
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束书简</param>
        /// <param name="maxTotalAmount">最大订单金额</param>
        /// <param name="minTotalAmount">最小订单金额</param>
        /// <param name="customerName">客户名称</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="includeDetails"></param>
        /// <param name="isGetTotalCount">是否获取总记录条数</param>
        /// <returns></returns>
        public async Task<PageData<SaleOrder>> GetPagingListAsync(
            string orderNo = null
            , SaleOrderStatus? orderStatus = null
            , DateTime? beginTime = null
            , DateTime? endTime = null
            , decimal? maxTotalAmount = null
            , decimal? minTotalAmount = null
            , string customerName = null
            , int skipCount = 0
            , int maxResultCount = int.MaxValue
            , bool includeDetails = true
            , bool isGetTotalCount = true)
        {
            PageData<SaleOrder> result = new PageData<SaleOrder>();

            var query = GetQueryable()
               .WhereIf(!string.IsNullOrWhiteSpace(orderNo), x => x.OrderNo == orderNo)
               .WhereIf(orderStatus.HasValue, x => x.OrderStatus == orderStatus.Value)
               // 由于 customerName 是自定义扩展字段，存储在 ExtraProperties 字段中，无法在过滤sql中
               //.WhereIf(!string.IsNullOrWhiteSpace(customerName), x => x. == customerName)
               .WhereIf(maxTotalAmount.HasValue, x => x.TotalAmount <= maxTotalAmount.Value)
               .WhereIf(minTotalAmount.HasValue, x => x.TotalAmount >= minTotalAmount.Value)
               .WhereIf(beginTime.HasValue, x => x.OrderTime > beginTime.Value)
               .WhereIf(endTime.HasValue, x => x.OrderTime < endTime.Value);

            result.Items = await query.IncludeDetails(includeDetails).PageBy(skipCount, maxResultCount).ToListAsync();
            if (isGetTotalCount)
            {
                result.TotalCount = await query.LongCountAsync();
            }

            return result;
        }

    }
}
