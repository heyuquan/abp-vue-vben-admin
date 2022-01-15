using Leopard.Requests;
using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.Dto.SaleOrders;
using Mk.DemoB.IAppService;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Mk.DemoB.DataFilterAppService
{
    // https://docs.abp.io/zh-Hans/abp/latest/Data-Filtering
    // 实现自定义过滤的最佳方法是重写你的 DbContext 的 CreateFilterExpression
    // EG：
    //protected bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

    //protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
    //{
    //    var expression = base.CreateFilterExpression<TEntity>();

    //    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    //    {
    //        Expression<Func<TEntity, bool>> isActiveFilter =
    //            e => !IsActiveFilterEnabled || EF.Property<bool>(e, "IsActive");
    //        expression = expression == null
    //            ? isActiveFilter
    //            : CombineExpressions(expression, isActiveFilter);
    //    }

    //    return expression;
    //}

    [Route("api/demob/datafilter")]
    public class DataFilterAppService : DemoBAppService
    {
        private readonly IDataFilter _dataFilter;
        private readonly ISaleOrderAppService _saleOrderAppService;

        public DataFilterAppService(
            IDataFilter dataFilter,
            ISaleOrderAppService saleOrderAppService
            )
        {
            _dataFilter = dataFilter;
            _saleOrderAppService = saleOrderAppService;
        }

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("not-contain-delete-count")]
        public virtual async Task<ServiceResult<long>> GetSaleOrderCountAsync(GetSaleOrderPagingRequest req)
        {
            ServiceResult<long> result = new ServiceResult<long>(CorrelationIdIdProvider.Get());

            ServiceResult<PagedResultDto<SaleOrderDto>> retValue = await _saleOrderAppService.GetOrderPagingAsync(req);
            result.SetSuccess(retValue.Data.TotalCount);

            return result;
        }

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("contain-delete-count")]
        public virtual async Task<ServiceResult<long>> GetAllSaleOrderCountAsync(GetSaleOrderPagingRequest req)
        {
            ServiceResult<long> result = new ServiceResult<long>(CorrelationIdIdProvider.Get());

            using (_dataFilter.Disable<ISoftDelete>())
            {
                ServiceResult<PagedResultDto<SaleOrderDto>> retValue = await _saleOrderAppService.GetOrderPagingAsync(req);
                result.SetSuccess(retValue.Data.TotalCount);
            }

            return result;
        }
    }
}
