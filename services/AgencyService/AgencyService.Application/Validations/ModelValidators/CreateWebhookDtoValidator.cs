using AgencyService.Application.Models.Webhooks;
using FluentValidation;

namespace AgencyService.Application.Validations.ModelValidators
{
    public class CreateWebhookDtoValidator : AbstractValidator<CreateWebhookDto>
    {
        public CreateWebhookDtoValidator()
        {
            RuleFor(_ => _.WebhookId).NotEmpty().GreaterThan(0);
            RuleFor(_ => _.AgencyId).NotEmpty().GreaterThan(0);
            RuleFor(_ => _.Url)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Maximum length of URL should not be greater than 150 characters.");
        }
    }
}
