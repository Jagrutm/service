using AccountProcessService.Application.Models;
using FluentValidation;

namespace AccountProcessService.Application.Validations.ModelValidators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(z => z.Name).NotEmpty();
            RuleFor(z => z.AccountNumber)
                .NotEmpty()
                .MaximumLength(8)
                .WithMessage("Account number shouldn't have more than 8 characters.");
            RuleFor(z => z.SortCode)
               .NotEmpty()
               .MaximumLength(6)
               .WithMessage("Sort code shouldn't have more than 6 characters.");
        }
    }
}
