using Leopard.Paging;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.ExchangeRateMgr
{
    public interface IExchangeRateRepository : IBasicRepository<ExchangeRate, Guid>
    {

        /// <summary>
        /// 获取汇率的分页数据
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="captureBatchNumber">抓取批次</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<PageData<ExchangeRate>> GetPagingAsync(
            string currencyCodeFrom = null
            , string currencyCodeTo = null
            , DateTime? beginTime = null
            , DateTime? endTime = null
            , string captureBatchNumber = null
            , int skipCount = 0
            , int maxResultCount = int.MaxValue
            , bool isGetTotalCount = true);

    }
}
