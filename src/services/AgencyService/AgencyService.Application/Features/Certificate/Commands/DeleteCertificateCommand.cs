using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Commands
{
    public class DeleteCertificateCommand : IRequest<int>
    {
        public int CertificateId { get; set; }

        public Guid AgencyId { get; set; }
    }

    public class DeleteCertificateCommandHandler : IRequestHandler<DeleteCertificateCommand, int>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IAgencyValidator _agencyValidator;
        private readonly ICertificateValidator _certificateValidator;

        public DeleteCertificateCommandHandler(
            ICertificateRepository certificateRepository,
            IAgencyValidator agencyValidator,
            ICertificateValidator certificateValidator)
        {
            _certificateRepository = certificateRepository;
            _agencyValidator = agencyValidator;
            _certificateValidator = certificateValidator;
        }

        public async Task<int> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);

            var certificateToDelete = await _certificateRepository.GetByIdAsync(request.CertificateId);

            _certificateValidator.ValidateCertificateIsNotNull(certificateToDelete);
            
            await _certificateRepository.DeleteAsync(certificateToDelete);
            return request.CertificateId;
        }
    }
}
