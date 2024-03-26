using FluentValidation;

namespace AgencyService.Application.Features.Agency.Commands.UpdateAgency;

public class UpdateAgencyCommandValidator : AbstractValidator<UpdateAgencyCommand>
{
    public UpdateAgencyCommandValidator()
    {
        RuleFor(p => p.AgencyId)
            .NotEmpty().WithMessage("{AgencyID} is required.")
            .NotNull();

        RuleFor(p => p.AgencyName)
            .NotEmpty().WithMessage("{AguencyName} is required.")
            .NotNull()
            .MaximumLength(255).WithMessage("{AguencyName} must not exceed 255 characters.");

        RuleFor(p => p.RegistrationDate)
            .NotEmpty().WithMessage("{RegistrationDate} is required.");

        RuleFor(p => p.AgencyCode)
            .NotEmpty().WithMessage("{AgencyCode} is required.")
            .NotNull()
            .MaximumLength(6).WithMessage("{AgencyCode} must not exceed 6 characters.");
    }

}
