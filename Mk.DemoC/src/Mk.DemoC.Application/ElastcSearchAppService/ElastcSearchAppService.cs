using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElastcSearchAppService : DemoCAppService
    {
        private readonly ILogger<ElastcSearchAppService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ElastcSearchAppService(
            ILogger<ElastcSearchAppService> logger
            , IHttpClientFactory clientFactory
            )
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        //https://www.zyccst.com/yaocai-277.html		狗鞭
        //https://www.zyccst.com/yaocai-786.html		广金钱草
        //https://www.zyccst.com/yaocai-192.html		枸杞子
        //https://www.zyccst.com/yaocai-526.html		石斛
        public async Task CaptureProductSpuDocAsync()
        {
            HttpClient client = _clientFactory.CreateClient();

            string[] urls = new string[]
            {
                "https://www.zyccst.com/yaocai-277.html",
                "https://www.zyccst.com/yaocai-786.html",
                "https://www.zyccst.com/yaocai-192.html",
                "https://www.zyccst.com/yaocai-526.html"
            };

            foreach (var url in urls)
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_M_lblToAmount']");
            }



            if (node != null)
            {
                decimal buyPrice = decimal.Parse(node.InnerText);

                ExchangeRate exchangeRate = new ExchangeRate(GuidGenerator.Create(), currencyCodeFrom, currencyCodeTo, buyPrice, Clock.Now);
                exchangeRate.DataFromUrl = url;

                return exchangeRate;
            }
            else
            {
                _logger.LogWarning($"来源币别{currencyCodeFrom}到目的币别{currencyCodeTo}抓取不到汇率。url:{url}");
                return null;
            }
        }
    }
}
