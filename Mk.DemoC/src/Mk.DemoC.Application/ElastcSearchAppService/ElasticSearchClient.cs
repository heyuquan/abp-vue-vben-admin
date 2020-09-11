using Microsoft.Extensions.Configuration;
using Mk.DemoC.ElastcSearchAppService.Documents;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElasticSearchClient : ITransientDependency
    {
        public const string MALL_SEARCH_PRODUCT = "mall.search.product";

        private readonly IElasticClient client;        
        private readonly IConfiguration _configuration;

        public ElasticSearchClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var node = new Uri(_configuration["ElasticConfiguration:Uri"]);
            var settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);

            this.InitIndex();
        }

        private void InitIndex()
        {
            client.Indices.Create(MALL_SEARCH_PRODUCT, c => c
                //.Settings(s => s
                //    .Analysis(a => a
                //        .Normalizers(n => n.Custom("lowercase", cn => cn.Filters("lowercase")))
                //        )
                //    )
                .Map<ProductSpuDocument>(mm => mm
                    .AutoMap())
                //.Map(m => m.DynamicTemplates(dt => dt.DynamicTemplate("keyword_to_lowercase", t => t
                //            .MatchMappingType("string")
                //            .Mapping(map => map.Keyword(k => k.Normalizer("lowercase")))
                //         )
                //       ))
            );
        }

        public IElasticClient Get()
        {
            return client;
        }
    }
}
