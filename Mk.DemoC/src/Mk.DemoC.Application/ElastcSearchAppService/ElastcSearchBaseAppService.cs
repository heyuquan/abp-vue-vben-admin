using HtmlAgilityPack;
using Leopard.Paging;
using Leopard.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mk.DemoC.Domain.Consts.ElastcSearchs;
using Mk.DemoC.Dto.ElastcSearchs;
using Mk.DemoC.SearchDocumentMgr;
using Mk.DemoC.SearchDocumentMgr.Documents;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Nest;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Mk.DemoC.ElastcSearchAppService
{
    public class ElastcSearchBaseAppService : DemoCAppService
    {
        protected readonly IElasticClient client;

        public ElastcSearchBaseAppService(ElasticSearchClient elasticSearchClient)
        {
            client = elasticSearchClient.Get();
        }

        [HttpPost("doc/search")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServiceResult<PagedResultDto<ProductSpuDocumentDto>>))]
        public async Task<ServiceResult<PagedResultDto<ProductSpuDocumentDto>>> SearchAsync(EsSearchRequest req)
        {
            ServiceResult<PagedResultDto<ProductSpuDocumentDto>> ret = new ServiceResult<PagedResultDto<ProductSpuDocumentDto>>(IdProvider.Get());

            var shouldQuerys = new List<Func<QueryContainerDescriptor<ProductSpuDocument>, QueryContainer>>();
            shouldQuerys.Add(t => t.Match(f => f
                                  .Field(x => x.SpuName)
                                  .Query(req.Keyword)
                                  .Boost(4)
                                  .Analyzer(ElastcSearchAnazer.IK_SMART)
                            ));
            shouldQuerys.Add(t => t.Match(f => f
                                            .Field(x => x.SpuCode)
                                            .Boost(5.5)
                                            .Query(req.Keyword)
                                            .Analyzer(ElastcSearchAnazer.IK_SMART)
                                        ));
            shouldQuerys.Add(t => t.Match(f => f
                                            .Field(x => x.SumSkuCode)
                                            .Boost(5)
                                            .Query(req.Keyword)
                                            .Analyzer(ElastcSearchAnazer.IK_SMART)
                                        ));
            shouldQuerys.Add(t => t.Match(f => f
                                            .Field(x => x.SpuKeywords)
                                            .Query(req.Keyword)
                                            .Boost(3.5)
                                            .Analyzer(ElastcSearchAnazer.IK_SMART)
                                        ));
            shouldQuerys.Add(t => t.Match(f => f
                                            .Field(x => x.SumSkuKeywords)
                                            .Query(req.Keyword)
                                            .Boost(3)
                                            .Analyzer(ElastcSearchAnazer.IK_SMART)
                                        ));

            var mustFilters = new List<Func<QueryContainerDescriptor<ProductSpuDocument>, QueryContainer>>();
            if (!string.IsNullOrEmpty(req.Currency))
            {
                mustFilters.Add(t => t.Term(f => f.Currency, req.Currency));
            }
            if (!string.IsNullOrEmpty(req.Brand))
            {
                mustFilters.Add(t => t.Term(f => f.Brand, req.Brand));
            }
            if (req.MinPrice.HasValue)
            {
                mustFilters.Add(t => t.Range(r => r.Field(f => f.MinPrice).GreaterThanOrEquals(req.MinPrice.Value)));
            }
            if (req.MaxPrice.HasValue)
            {
                mustFilters.Add(t => t.Range(r => r.Field(f => f.MaxPrice).LessThanOrEquals(req.MaxPrice.Value)));
            }

            var rp = client.Search<ProductSpuDocument>(s => s
                            .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                            .MinScore(0.02)
                            .Skip(req.SkipCount)
                            .Size(req.MaxResultCount)
                            .Query(q => q
                                .Bool(b => b
                                    .Should(shouldQuerys).MinimumShouldMatch(new MinimumShouldMatch(1))
                                    .Filter(f => f
                                        .Bool(fb => fb.Must(mustFilters))

                                    )
                                )

                            )
                        );
            var productSpuDocuments = rp.Documents.ToList();

            PagedResultDto<ProductSpuDocumentDto> pageDtos = new PagedResultDto<ProductSpuDocumentDto>();
            pageDtos.Items = ObjectMapper.Map<List<ProductSpuDocument>, List<ProductSpuDocumentDto>>(productSpuDocuments);
            pageDtos.TotalCount = rp.Total;
            ret.SetSuccess(pageDtos);
            return ret;
        }

        [HttpDelete("doc/delete/all")]
        public async Task DeleteAsync()
        {
            string id = "39f792fdc909816aeda1bbb97faa18a5";

            DocumentPath<ProductSpuDocument> deletePath = new DocumentPath<ProductSpuDocument>(id);
            var rp1 = await client.DeleteAsync(deletePath, d => d.Index(ElasticSearchClient.MALL_SEARCH_PRODUCT));

            if (rp1.OriginalException != null)
            {
                throw rp1.OriginalException;
            }

            var rp2 = await client.DeleteByQueryAsync<ProductSpuDocument>(
                     s => s
                         .Index(ElasticSearchClient.MALL_SEARCH_PRODUCT)
                         .Query(q => q.MatchAll()
                         //.Match(m => m.Field(x => x.SumSkuCode).Query("A0011"))
                         )
                     );
            if (rp2.OriginalException != null)
            {
                throw rp2.OriginalException;
            }
        }
    }
}
