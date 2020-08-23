using Leopard.Results;
using Mk.DemoB.BookMgr.Entities;
using Mk.DemoB.Dto;
using System.Threading.Tasks;

namespace Mk.DemoB.DtoValidAppService
{
    // 注意：方法应该是 virtual
    // ABP框架使用动态代理/拦截系统来执行验证.为了使其工作,你的方法应该是 virtual 的,服务应该被注入并通过接口(如IMyService)使用.
    // 定义 IValidationEnabled 向任意类添加自动验证. 所有的应用服务都实现了该接口,所以它们会被自动验证.
    // abp做了如下自动验证
    // 1、dto上的数据注解验证
    // 2、Dto实现IValidatableObject接口，并在 Validate 方法中写验证代码。（但不推荐，原因：应保持简单的DTO,他们的目的是传输数据）

    // 怎么做手动验证？
    // 1、Volo.Abp.FluentValidation 在Abp自动验证时，会找到AbstractValidator类，并进行验证
    // eg： Validator/CreateBookDtoValidator .cs

    // FluentValidation使用资料：https://www.xcode.me/post/5849

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
