
using FluentValidation;

namespace AgencyService.Application.Features.Agency.Commands.CreateAgency;

public class CreateAgencyCommandValidator : AbstractValidator<CreateAgencyCommand>
{
    public CreateAgencyCommandValidator()
    {
        RuleFor(p => p.AgencyName)
            .NotEmpty().WithMessage("{AgencyName} is required.")
            .NotNull()
            .MaximumLength(255).WithMessage("{AgencyName} must not exceed 255 characters.");

        RuleFor(p => p.RegistrationDate)
            .NotEmpty().WithMessage("{RegistrationDate} is required.");

        RuleFor(p => p.AgencyCode)
            .NotEmpty().WithMessage("{AgencyCode} is required.")
            .NotNull()
            .MaximumLength(6).WithMessage("{AgencyCode} must not exceed 6 characters.");
    }
}
