using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Services;

namespace Mk.DemoB.BookMgr
{
    public class BookManager: DomainService
    {
        public BookManager()
        {
            
        }

        public async Task<PagedResultDto<BookDto>> GetBookListContainDeleted(GetBookListRequestDto input)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                return await _bookRepository
            }
        }
    }
}
