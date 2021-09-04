using AutoMapper;
using Mk.DemoC.Dto.ElastcSearchs;
using Mk.DemoC.SearchDocumentMgr.Documents;
using Mk.DemoC.SearchDocumentMgr.Entities;
using Volo.Abp.AutoMapper;

namespace Mk.DemoC
{
    public class DemoCApplicationAutoMapperProfile : Profile
    {
        public DemoCApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<ProductSpuDoc, ProductSpuDocument>();
            CreateMap<ProductSpuDocument, ProductSpuDocumentDto>();
        }
    }
}