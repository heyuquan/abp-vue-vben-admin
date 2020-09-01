using Leopard.Results;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using System.Threading.Tasks;

namespace Mk.DemoB.DtoValidAppService
{


    public class DtoValidAppService : DemoBAppService
    {
        public DtoValidAppService()
        {
           
        }

        /// <summary>
        /// 验证输入参数Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResult<BookDto>> CreateBookAsync(CreateBookRequestDto input)
        {
            ServiceResult<BookDto> ret = new ServiceResult<BookDto>();
            Book book = new Book
            (
                GuidGenerator.Create(),
                input.Name,
                input.Price
            );

            ret.SetSuccess(ObjectMapper.Map<Book, BookDto>(book));
            return ret;
        }
    }
}
