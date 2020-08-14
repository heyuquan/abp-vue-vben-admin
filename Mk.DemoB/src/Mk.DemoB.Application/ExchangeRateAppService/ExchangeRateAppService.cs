using HtmlAgilityPack;
using Mk.DemoB.ExchangeRate;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mk.DemoB.ExchangeRateAppService
{
    /// <summary>
    /// 汇率
    /// </summary>
    public class ExchangeRateAppService : DemoBAppService
    {
        public readonly IHttpClientFactory _clientFactory;

        public ExchangeRateAppService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task CaptureRate()
        {
            HttpClient client = _clientFactory.CreateClient("rate");


            var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("pjname", "美元")
                };

            string html = string.Empty;
            var content = new FormUrlEncodedContent(body);
            HttpResponseMessage response = client.PostAsync("/search/whpj/search_cn.jsp", content).Result;
            html = response.Content.ReadAsStringAsync().Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

        }
    }
}
