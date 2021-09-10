using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElasticSearchLowClient : ITransientDependency
    {
        public ElasticLowLevelClient client { get; }

        private readonly IConfiguration _configuration;
        private readonly IJsonSerializer _jsonSerializer;

        public ElasticSearchLowClient(
            IConfiguration configuration
            , IJsonSerializer jsonSerializer
            )
        {
            _configuration = configuration;
            _jsonSerializer = jsonSerializer;
            client = InitClient();
        }

        private ElasticLowLevelClient InitClient()
        {
            var node = new Uri(_configuration["ElasticSearch:Uri"]);
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticLowLevelClient(settings);

            return client;
        }

        private void ResponseValidate(StringResponse response)
        {
            if (response.Success == false)
            {
                throw new BusinessException(message: response.Body);
            }
        }

        public async Task<string> Index(string index, string id, PostData body)
        {
            var response = await client.IndexAsync<StringResponse>(index, id, body);

            ResponseValidate(response);
            return response.Body;
        }

        public async Task<List<string>> SearchWithHighLight(string index, string query)
        {
            var response = await client.SearchAsync<StringResponse>(
                index,
                PostData.Serializable(
                    new
                    {
                        from = 0,
                        size = 100,
                        query = new
                        {
                            match = new { content = query }
                        },
                        hightlight = new
                        {
                            pre_tags = new[] { "<tag1>", "<tag2>" },
                            post_tags = new[] { "/<tag1>", "/<tag2>" },
                            fields = new
                            {
                                content = new { }
                            }
                        }
                    })
                );

            ResponseValidate(response);

            var responseJson = _jsonSerializer.Deserialize<JObject>(response.Body);

            var hits = responseJson["hits"]["hits"] as JArray;

            var result = new List<string>();

            foreach (var hit in hits)
            {
                var id = hit["_id"].ToObject<string>();

                result.Add(id);
            }

            return result;
        }

        public async Task Delete(string index, string id)
        {
            var response = await client.DeleteAsync<StringResponse>(index, id);

            ResponseValidate(response);
        }
    }
}
