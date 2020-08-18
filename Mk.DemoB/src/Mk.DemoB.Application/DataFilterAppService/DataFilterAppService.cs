using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.BookMgr;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.DataFilterAppService
{
    // https://docs.abp.io/zh-Hans/abp/latest/Data-Filtering
    // 实现自定义过滤的最佳方法是为重写你的 DbContext 的 CreateFilterExpression
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

    public class DataFilterAppService : DemoBAppService
    {
        private readonly IDataFilter _dataFilter;
        private readonly IBookRepository _bookRepository;

        public DataFilterAppService(
            IDataFilter dataFilter,
            IBookRepository bookRepository
            )
        {
            _dataFilter = dataFilter;
            _bookRepository = bookRepository;
        }

        // ListResultDto

        [Route("booklist")]
        [HttpGet]
        public async Task<PagedResultDto<BookDto>> GetBookListContainDeleted(GetBookListRequestDto input)
        {
            List<Book> books = null;
            long count = 0;
            using (_dataFilter.Disable<ISoftDelete>())
            {
                books = await _bookRepository.GetListAsync(input.MinPrice, input.MaxPrice, input.MaxResultCount, input.SkipCount);
                count = await _bookRepository.GetCountAsync(input.MinPrice, input.MaxPrice);
            }

            var dtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);
            return new PagedResultDto<BookDto>(count, dtos);
        }


    }
}
