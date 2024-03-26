using AccountProcessService.Application.Models;
using FluentValidation;

namespace AccountProcessService.Application.Validations.ModelValidators
{
    public class CreateAccountsDtoValidator : AbstractValidator<CreateAccountsDto>
    {
        public CreateAccountsDtoValidator()
        {
            RuleForEach(z => z.Accounts).SetValidator(new CreateAccountDtoValidator());
        }
    }
}
