using Leopard.Paging;
using Microsoft.EntityFrameworkCore;
using Mk.DemoB.EntityFrameworkCore;
using Mk.DemoB.ExchangeRateMgr;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp;

namespace Mk.DemoB.Repository
{
    public class ExchangeRateRepository : EfCoreRepository<DemoBDbContext, ExchangeRate, Guid>, IExchangeRateRepository
    {
        public ExchangeRateRepository(IDbContextProvider<DemoBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// 获取汇率的分页数据
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="captureBatchNumber">抓取批次</param>
        /// <param name="sorting">Eg：Name asc,Id desc</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        public async Task<PageData<ExchangeRate>> GetPagingAsync(
            string currencyCodeFrom = null
            , string currencyCodeTo = null
            , DateTime? beginTime = null
            , DateTime? endTime = null
            , string captureBatchNumber = null
            , string sorting = null
            , int skipCount = 0
            , int maxResultCount = int.MaxValue
            , bool isGetTotalCount = true)
        {
            PageData<ExchangeRate> result = new PageData<ExchangeRate>();
            var query = (await GetQueryableAsync())
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeFrom), x => x.CurrencyCodeFrom == currencyCodeFrom)
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeTo), x => x.CurrencyCodeTo == currencyCodeTo)
                .WhereIf(beginTime.HasValue, x => x.CaptureTime >= beginTime.Value)
                .WhereIf(endTime.HasValue, x => x.CaptureTime <= endTime.Value)
                .WhereIf(!String.IsNullOrWhiteSpace(captureBatchNumber), x => x.CaptureBatchNumber == captureBatchNumber);

            if (isGetTotalCount)
            {
                result.TotalCount = await query.LongCountAsync();
            }

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

    }
}
