﻿using Leopard.Paging;
using Mk.DemoB.Enums.SaleOrder;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.SaleOrderMgr
{
    public interface ISaleOrderRepository : IBasicRepository<SaleOrder, Guid>
    {
        /// <summary>
        /// 根据订单编号获取订单
        /// </summary>
        /// <returns></returns>
        Task<SaleOrder> GetByOrderNoAsync(string orderNo);

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
        Task<PageData<SaleOrder>> GetPagingListAsync(
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
            , bool isGetTotalCount = true);
    }
}