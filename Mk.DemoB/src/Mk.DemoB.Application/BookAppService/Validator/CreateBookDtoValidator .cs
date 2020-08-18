using FluentValidation;
using Mk.DemoB.Dto;

namespace Mk.DemoB.BookAppService.Validator
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookRequestDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Name).NotEqual("string");
            RuleFor(x => x.Price).ExclusiveBetween(0.0M, 999.0M);
        }
    }
}
