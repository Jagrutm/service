using AgencyService.Application.Features.Certificate.Commands;
using FluentValidation;

namespace AgencyService.Application.Validations.ModelValidators
{
    public class CreateCertificateCommandValidator : AbstractValidator<CreateCertificateCommand>
    {
        public CreateCertificateCommandValidator()
        {
            RuleFor(_ => _.AgencyId).NotNull().NotEmpty();
            RuleFor(_=>_.CertificateName).NotEmpty();
            RuleFor(_=>_.CertificateKey).NotEmpty();
            RuleFor(_ => _.Type).NotEmpty();

            RuleFor(_=>_.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("Start date must be greater than or equal to Today's date.");

            RuleFor(_=>_.ExpiryDate)
                .NotEmpty()
                .GreaterThan(_=>_.StartDate)
                .WithMessage("Expiry date must be greater than start date.");
        }
    }
}
