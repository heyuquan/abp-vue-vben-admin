using AutoMapper;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using Mk.DemoB.Dto.ExchangeRates;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Mk.DemoB.SaleOrderMgr.Entities;
using Volo.Abp.AutoMapper;

namespace Mk.DemoB.MappingProfile
{
    public class MkDemoBApplicationMapperProfile : Profile
    {
        public MkDemoBApplicationMapperProfile()
        {
            // 忽略字段：Volo.Abp.AutoMapper 中定义了很多忽略扩展方法 AutoMapperExpressionExtensions
            //CreateMap<ExchangeRate, ExchangeRateDto>()
            //    .Ignore(x => x.BuyPrice);

            // 应用层的Mapping一般是  domain中的实体 到 Dto 的转换

            CreateMap<Book, BookDto>()
                .MapExtraProperties();

            CreateMap<ExchangeRate, ExchangeRateDto>();

            CreateMap<CaptureCurrency, CaptureCurrencyDto>();

            CreateMap<SaleOrder, SaleOrderDto>()
                .MapExtraProperties();  // 会映射实体中的 Dictionary<string, object> ExtraProperties 属性

            CreateMap<SaleOrderDetail, SaleOrderDetailDto>();
        }
    }
}
