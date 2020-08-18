using Leopard.Result;
using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.BookMgr;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mk.DemoB.BookAppService
{
    public class BookAppService : DemoBAppService
    {
        private readonly IBookRepository _bookRepository;
        public BookAppService(
            IBookRepository bookRepository
            )
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public virtual async Task<ServiceResult<BookDto>> CreateAsync(CreateBookRequestDto input)
        {
            ServiceResult<BookDto> ret = new ServiceResult<BookDto>();
            Book newBook = new Book
            (
                GuidGenerator.Create(),
                input.Name,
                input.Price
            );

            var book = await _bookRepository.InsertAsync(newBook);

            ret.SetSuccess(ObjectMapper.Map<Book, BookDto>(book));
            return ret;
        }

        [HttpPost]
        public virtual async Task<ServiceResult> RemoveAsync(Guid id)
        {
            ServiceResult ret = new ServiceResult();

            await _bookRepository.DeleteAsync(id);

            ret.SetSuccess();
            return ret;
        }
    }
}
