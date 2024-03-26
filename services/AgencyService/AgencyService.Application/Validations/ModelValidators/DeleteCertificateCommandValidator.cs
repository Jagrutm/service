using AgencyService.Application.Features.Certificate.Commands;
using FluentValidation;

namespace AgencyService.Application.Validations.ModelValidators
{
    public class DeleteCertificateCommandValidator : AbstractValidator<DeleteCertificateCommand>
    {
        public DeleteCertificateCommandValidator()
        {
            RuleFor(_=>_.AgencyId).NotNull().NotEmpty();
            RuleFor(_=>_.CertificateId).NotNull().NotEmpty();
        }
    }
}
