﻿using Leopard.Result;
using Mk.DemoB.Dto.DtoValid;
using Mk.DemoB.DtoValid;
using System;
using System.Collections.Generic;
using System.Text;
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

    public class DtoValidAppService : DemoBAppService
    {
        private readonly DtoValidManager _dtoValidManager;
        public DtoValidAppService(DtoValidManager dtoValidManager)
        {
            _dtoValidManager = dtoValidManager;
        }

        public virtual async Task<ServiceResult<BookDto>> CreateBookAsync(CreateBookDto dto)
        {
            ServiceResult<BookDto> ret = new ServiceResult<BookDto>();
            Book book = new Book
            (
                GuidGenerator.Create(),
                dto.Name,
                dto.Price
            );

            var result = await _dtoValidManager.CreateBookAsync(book);
            ret.SetSuccess(ObjectMapper.Map<Book, BookDto>(result));
            return ret;
        }
    }
}
