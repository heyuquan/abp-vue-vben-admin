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

        /// <summary>
        /// 获取 Book 数据（包含 软删除）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("booklist")]
        [HttpGet]
        public async Task<PagedResultDto<BookDto>> GetBookListContainDeleted(GetBookListRequestDto input)
        {
            List<Book> books = null;
            long count = 0;
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var pageData= await _bookRepository.GetPagingListAsync(input.MinPrice, input.MaxPrice, input.MaxResultCount, input.SkipCount);
                books = pageData.Items;
                count = pageData.TotalCount;
            }

            var dtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);
            return new PagedResultDto<BookDto>(count, dtos);
        }


    }
}
