using Mk.DemoC.SearchDocumentMgr.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoC.SearchDocumentMgr
{
    public interface IProductSpuDocRepository : IBasicRepository<ProductSpuDoc, Guid>
    {
        Task DeleteAllAsync();
    }
}
