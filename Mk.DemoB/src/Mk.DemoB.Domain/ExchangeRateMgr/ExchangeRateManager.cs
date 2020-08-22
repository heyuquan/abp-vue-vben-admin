using HtmlAgilityPack;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;

namespace Mk.DemoB.ExchangeRateMgr
{
    public class ExchangeRateManager : DomainService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepository<ExchangeRate, Guid> _exchangeRateRepository;
        private readonly IRepository<CaptureCurrency, Guid> _captureCurrencyRepository;
        private readonly IRepository<ExchangeRateCaptureBatch, Guid> _exchangeRateCaptureBatchRepository;

        public ExchangeRateManager(
            IHttpClientFactory clientFactory
            , IRepository<ExchangeRate, Guid> exchangeRateRepository
            , IRepository<CaptureCurrency, Guid> captureCurrencyRepository
            , IRepository<ExchangeRateCaptureBatch, Guid> exchangeRateCaptureBatchRepository
            )
        {
            _clientFactory = clientFactory;
            _exchangeRateRepository = exchangeRateRepository;
            _captureCurrencyRepository = captureCurrencyRepository;
            _exchangeRateCaptureBatchRepository = exchangeRateCaptureBatchRepository;
        }


        // ID 和 Class h2[@id='test'][@class='test']
        // Html Agility Pack如何快速实现解析Html
        // https://www.cnblogs.com/xuliangxing/p/8004403.html
        /// <summary>
        /// 抓取指定的汇率
        /// </summary>
        /// <returns></returns>
        private async Task<ExchangeRate> CaptureOneRateAsync(string currencyCodeFrom = "CNY", string currencyCodeTo = "USD")
        {

            HttpClient client = _clientFactory.CreateClient();

            string url = $"https://www.exchange-rates.org/converter/{currencyCodeFrom}/{currencyCodeTo}/1";
            HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
            string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_M_lblToAmount']");

            decimal buyPrice = decimal.Parse(node.InnerText);

            ExchangeRate exchangeRate = new ExchangeRate(GuidGenerator.Create(), currencyCodeFrom, currencyCodeTo, buyPrice, Clock.Now);
            exchangeRate.DataFromUrl = url;

            return exchangeRate;
        }

        /// <summary>
        /// 抓取所有CaptureCurrency表中指定的汇率
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExchangeRate>> CaptureAllRateAndSaveAsync()
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            // #、获取要获取的币种源数据
            List<CaptureCurrency> captureCurrencys = await _captureCurrencyRepository.GetListAsync();

            string captureBatchNumber = Guid.NewGuid().ToString("N");
            ExchangeRateCaptureBatch batch = new ExchangeRateCaptureBatch(GuidGenerator.Create(), captureBatchNumber, Clock.Now);
            // #、抓取汇率
            if (captureCurrencys.Any())
            {

                var exchangeRates = new ConcurrentBag<ExchangeRate>();

                var tasks = captureCurrencys.Select(async item =>
                {
                    var exchangeRate = await this.CaptureOneRateAsync(item.CurrencyCodeFrom, item.CurrencyCodeTo);
                    exchangeRate.CaptureBatchNumber = captureBatchNumber;
                    exchangeRates.Add(exchangeRate);
                }).ToArray();

                await Task.WhenAll(tasks);

                result = exchangeRates.ToList();

                // #、保存抓取的汇率
                // #、保存抓取批次
                if (result.Any())
                {
                    result.ForEach(async item => await _exchangeRateRepository.InsertAsync(item));
                    batch.IsSuccess = true;
                }
                else
                {
                    batch.IsSuccess = false;
                    batch.Remark = "没有抓取到任何数据";
                }
            }
            else
            {
                batch.IsSuccess = false;
                batch.Remark = "没有要抓取的币别主数据";
            }
            await _exchangeRateCaptureBatchRepository.InsertAsync(batch);

            return result;
        }

        /// <summary>
        /// 获取最新批次的汇率数据
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="topCount">按时间排序，返回多少条数据。最新时间的数据为第一条</param>
        /// <returns></returns>
        public async Task<List<ExchangeRate>> GetTopRateAsync(string currencyCodeFrom, string currencyCodeTo, int topCount = 1)
        {
            var exchangeRates = await _exchangeRateRepository
                .Where(x => x.CurrencyCodeFrom == currencyCodeFrom && x.CurrencyCodeTo == currencyCodeTo)
                .OrderByDescending(x => x.CreationTime)
                .Take(topCount).ToListAsync();
            return exchangeRates;
        }

        /// <summary>
        /// 获取汇率的分页数据
        /// </summary>
        /// <param name="currencyCodeFrom">来源币种（eg：CNY）</param>
        /// <param name="currencyCodeTo">目的币种（eg：USD）</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        public async Task<List<ExchangeRate>> GetRatePagingAsync(string currencyCodeFrom, string currencyCodeTo
            , DateTime? beginTime, DateTime? endTime, [NotNull]int skipCount, [NotNull]int maxResultCount)
        {
            var exchangeRates = await _exchangeRateRepository
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeFrom), x => x.CurrencyCodeFrom == currencyCodeFrom)
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeTo), x => x.CurrencyCodeTo == currencyCodeTo)
                .WhereIf(beginTime.HasValue, x => x.CaptureTime >= beginTime.Value)
                .WhereIf(endTime.HasValue, x => x.CaptureTime <= endTime.Value)
                .OrderByDescending(x => x.CreationTime)
                .PageBy(skipCount, maxResultCount).ToListAsync();
            return exchangeRates;
        }

        public async Task<int> GetRatePagingCountAsync(string currencyCodeFrom, string currencyCodeTo
            , DateTime? beginTime, DateTime? endTime)
        {
            var count = await _exchangeRateRepository
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeFrom), x => x.CurrencyCodeFrom == currencyCodeFrom)
                .WhereIf(!String.IsNullOrWhiteSpace(currencyCodeTo), x => x.CurrencyCodeTo == currencyCodeTo)
                .WhereIf(beginTime.HasValue, x => x.CaptureTime >= beginTime.Value)
                .WhereIf(endTime.HasValue, x => x.CaptureTime <= endTime.Value)
                .CountAsync();
            return count;
        }
    }
}
