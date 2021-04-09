using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Leopard.Paging;

namespace Mk.DemoC.SearchDocumentMgr
{
    public interface IProductSpuDocRepository : IBasicRepository<ProductSpuDoc, Guid>
    {
        Task DeleteAllAsync();

        /// <summary>
        /// 获取 产品Spu 索引元数据
        /// </summary>
        /// <param name="sorting">Eg：Name asc,Id desc</param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="isGetTotalCount"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        Task<PageData<ProductSpuDoc>> GetPagingAsync(
           string sorting = null
           , int skipCount = 0
           , int maxResultCount = int.MaxValue
           , bool isGetTotalCount = true
           , bool isNoTracking = false);
    }
}
