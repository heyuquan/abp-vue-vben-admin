using AutoMapper;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using Mk.DemoB.Dto.ExchangeRates;
using Mk.DemoB.ExchangeRateMgr.Entities;

namespace Mk.DemoB.MappingProfile
{
    public class MkDemoBApplicationMapperProfile : Profile
    {
        public MkDemoBApplicationMapperProfile()
        {
            // 应用层的Mapping一般是  domain中的实体 到 Dto 的转换

            CreateMap<Book, BookDto>()
                .MapExtraProperties();

            CreateMap<ExchangeRate, ExchangeRateDto>();

            CreateMap<CaptureCurrency, CaptureCurrencyDto>();           

        }
    }
}
