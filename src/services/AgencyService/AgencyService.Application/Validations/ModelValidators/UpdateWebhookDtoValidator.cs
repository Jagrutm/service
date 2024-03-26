using AgencyService.Application.Models.Webhooks;
using FluentValidation;

namespace AgencyService.Application.Validations.ModelValidators
{
    public class UpdateWebhookDtoValidator : AbstractValidator<UpdateWebhookDto>
    {
        public UpdateWebhookDtoValidator()
        {
            RuleFor(_ => _.Url)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Maximum length of URL should not be greater than 150 characters.");
        }
    }
}
