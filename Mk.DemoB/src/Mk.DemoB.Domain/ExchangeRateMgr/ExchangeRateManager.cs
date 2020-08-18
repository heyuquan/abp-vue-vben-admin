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

namespace Mk.DemoB.ExchangeRateMgr
{
    public class ExchangeRateManager : DomainService
    {
        public readonly IHttpClientFactory _clientFactory;
        public readonly IRepository<ExchangeRate, Guid> _exchangeRateRepository;
        public readonly IRepository<CaptureCurrency, Guid> _captureCurrencyRepository;

        public ExchangeRateManager(
            IHttpClientFactory clientFactory
            , IRepository<ExchangeRate, Guid> exchangeRateRepository
            , IRepository<CaptureCurrency, Guid> captureCurrencyRepository
            )
        {
            _clientFactory = clientFactory;
            _exchangeRateRepository = exchangeRateRepository;
            _captureCurrencyRepository = captureCurrencyRepository;
        }

        /// <summary>
        /// 抓取指定的汇率
        /// </summary>
        /// <returns></returns>
        public async Task<ExchangeRate> CaptureOneRate(string currencyCodeFrom = "CNY", string currencyCodeTo = "USD")
        {

            HttpClient client = _clientFactory.CreateClient();

            string url = $"https://www.exchange-rates.org/converter/{currencyCodeFrom}/{currencyCodeTo}/1";
            HttpResponseMessage response = await client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_M_lblToAmount']");

            decimal buyPrice = decimal.Parse(node.InnerText);

            ExchangeRate exchangeRate = new ExchangeRate(GuidGenerator.Create(), currencyCodeFrom, currencyCodeTo, buyPrice);
            exchangeRate.DataFromUrl = url;

            return exchangeRate;
        }

        /// <summary>
        /// 抓取所有指定的汇率
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExchangeRate>> CaptureAllRateAndSaveAsync()
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            // #、获取要获取的币种源数据
            List<CaptureCurrency> captureCurrencys = await _captureCurrencyRepository.GetListAsync();

            // #、抓取汇率
            if (captureCurrencys.Any())
            {
                string captureBatchNumber = Guid.NewGuid().ToString("N");
                var exchangeRates = new ConcurrentBag<ExchangeRate>();
                Parallel.ForEach(captureCurrencys, async item =>
                 {
                     var exchangeRate = await this.CaptureOneRate(item.CurrencyCodeFrom, item.CurrencyCodeTo);
                     exchangeRate.CaptureBatchNumber = captureBatchNumber;
                     exchangeRates.Add(exchangeRate);
                 });
                result = exchangeRates.ToList();
            }
            if (result.Any())
            {
                await _exchangeRateRepository.(result);
            }
            return result;
        }
    }
}
