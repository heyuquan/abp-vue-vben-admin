using FluentValidation;
using Mk.DemoB.Dto.DtoValid;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.DtoValidAppService.Validator
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Name).NotEqual("string");
            RuleFor(x => x.Price).ExclusiveBetween(0.0M, 999.0M);
        }
    }
}
