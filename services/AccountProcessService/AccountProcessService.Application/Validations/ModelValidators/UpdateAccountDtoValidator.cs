using AccountProcessService.Application.Models;
using FluentValidation;

namespace AccountProcessService.Application.Validations.ModelValidators
{
    public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
    {
        public UpdateAccountDtoValidator()
        {
            RuleFor(z => z.Name).NotEmpty().WithMessage("Rax validation failed");
        }
    }
}
