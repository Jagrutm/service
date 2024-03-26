using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Queries
{
    public class VerifyCertificateQuery : IRequest<bool>
    {
        public Guid AgencyId { get; set; }

        public int CertificateId { get; set; }
    }

    public class VerifyCertificateQueryHandler : IRequestHandler<VerifyCertificateQuery, bool>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly ICertificateValidator _certificateValidator;
        private readonly IAgencyValidator _agencyValidator;
        
        public VerifyCertificateQueryHandler(
            ICertificateRepository certificateRepository,
            ICertificateValidator certificateValidator,
            IAgencyValidator agencyValidator)
        {
            _certificateRepository = certificateRepository;
            _certificateValidator = certificateValidator;
            _agencyValidator = agencyValidator;
        }

        public async Task<bool> Handle(VerifyCertificateQuery request, CancellationToken cancellationToken)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);
            var certificate = await _certificateRepository.GetByIdAsync(request.CertificateId);
            _certificateValidator.ValidateCertificateIsNotNull(certificate);

            return !certificate.IsRevoked;
        }
    }
}
