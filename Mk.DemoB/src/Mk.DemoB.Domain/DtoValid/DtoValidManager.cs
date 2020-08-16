using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Mk.DemoB.DtoValid
{
    public class DtoValidManager : IDomainService
    {
        public DtoValidManager()
        {
            
        }

        public virtual async Task<Book> CreateBookAsync(Book book)
        {
            return await Task.FromResult(book);
        }
    }
}
