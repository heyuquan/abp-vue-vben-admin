using Leopard.Paging;
using Microsoft.EntityFrameworkCore;
using Mk.DemoB.Domain.Enums.SaleOrders;
using Mk.DemoB.EntityFrameworkCore;
using Mk.DemoB.SaleOrderMgr;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            return await Task.FromResult((await GetQueryableAsync()).IncludeDetails().Where(x => x.OrderNo == orderNo).FirstOrDefault());
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
        /// <param name="sorting">Eg：Name asc,Id desc</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="includeDetails"></param>
        /// <param name="isGetTotalCount">是否获取总记录条数</param>
        /// <returns></returns>
        public async Task<PageData<SaleOrder>> GetPagingAsync(
            string orderNo = null
            , SaleOrderStatus? orderStatus = null
            , DateTime? beginTime = null
            , DateTime? endTime = null
            , decimal? maxTotalAmount = null
            , decimal? minTotalAmount = null
            , string sorting = null
            , int skipCount = 0
            , int maxResultCount = PagingConsts.ResultCount.Normal
            , bool includeDetails = true
            , bool isGetTotalCount = true)
        {
            PageData<SaleOrder> result = new PageData<SaleOrder>();

            IQueryable<SaleOrder> query = (await GetQueryableAsync())
               .WhereIf(!string.IsNullOrWhiteSpace(orderNo), x => x.OrderNo == orderNo)
               .WhereIf(orderStatus.HasValue, x => x.OrderStatus == orderStatus.Value)
               // 由于 customerName 是自定义扩展字段，存储在 ExtraProperties 字段中，无法在过滤sql中
               //.WhereIf(!string.IsNullOrWhiteSpace(customerName), x => x. == customerName)
               .WhereIf(maxTotalAmount.HasValue, x => x.TotalAmount <= maxTotalAmount.Value)
               .WhereIf(minTotalAmount.HasValue, x => x.TotalAmount >= minTotalAmount.Value)
               .WhereIf(beginTime.HasValue, x => x.OrderTime > beginTime.Value)
               .WhereIf(endTime.HasValue, x => x.OrderTime < endTime.Value);

            if (isGetTotalCount)
            {
                result.TotalCount = await query.LongCountAsync();
            }

            query = query.IncludeDetails(includeDetails);
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                query = query.OrderBy(sorting);
            }
            else 
            {
                query.OrderByDescending(x => x.Id);
            }

            result.Items = await query.PageBy(skipCount, maxResultCount).ToListAsync();

            return result;
        }

        // 重写了这个方法，Repository中的 方法参数 includeDetails 参数才有效果。  
        // 因为这个方法中指定了 实体 和 关联实体的关系，不指定系统也不知道要关联啥东西
        public override async Task<IQueryable<SaleOrder>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

    }
}
