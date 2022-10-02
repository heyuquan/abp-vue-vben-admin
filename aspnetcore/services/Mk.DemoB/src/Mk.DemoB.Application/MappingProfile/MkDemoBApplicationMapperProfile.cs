using AutoMapper;
using Mk.DemoB.Dto.ExchangeRates;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.ExchangeRateMgr.Entities;
using Mk.DemoB.SaleOrderMgr.Entities;
using Volo.Abp.Data;

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

            CreateMap<ExchangeRate, ExchangeRateDto>();

            CreateMap<CaptureCurrency, CaptureCurrencyDto>();

            CreateMap<SaleOrder, SaleOrderDto>()
                .MapExtraProperties()       // 会映射实体中的 Dictionary<string, object> ExtraProperties 属性
                .AfterMap((saleOrder,dto)=> {
                    dto.CustomerName = saleOrder.GetProperty<string>("CustomerName");   // 需要主动映射到独立字段上，不然只会以json的形式在ExtraProperties字段中返回到前台
                });  

            CreateMap<SaleOrderDetail, SaleOrderDetailDto>();
        }
    }
}
