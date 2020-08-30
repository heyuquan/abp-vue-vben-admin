using HtmlAgilityPack;
using JetBrains.Annotations;
using Mk.DemoB.ExchangeRateMgr.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

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

        
    }
}
