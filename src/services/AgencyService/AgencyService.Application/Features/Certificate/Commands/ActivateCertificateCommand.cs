using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Commands
{
    public class ActivateCertificateCommand : IRequest<bool>
    {
        public Guid AgencyId { get; set; }

        public int CertificateId { get; set; }
    }

    public class ActivateCertificateCommandHandler : IRequestHandler<ActivateCertificateCommand, bool>
    {
        private readonly IAgencyValidator _agencyValidator;
        private readonly ICertificateValidator _certificateValidator;
        private readonly ICertificateRepository _certificateRepository;

        public ActivateCertificateCommandHandler(
            IAgencyValidator agencyValidator,
            ICertificateValidator certificateValidator,
            ICertificateRepository certificateRepository)
        {
            _agencyValidator = agencyValidator;
            _certificateValidator = certificateValidator;
            _certificateRepository = certificateRepository;
        }

        public async Task<bool> Handle(ActivateCertificateCommand request, CancellationToken cancellationToken)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);

            var certificateToActivate = await _certificateRepository.GetByIdAsync(request.CertificateId);
            _certificateValidator.ValidateCertificateIsNotNull(certificateToActivate);

            certificateToActivate.IsActive = true;

            return await _certificateRepository.UpdateAsync(certificateToActivate);
        }
    }
}
