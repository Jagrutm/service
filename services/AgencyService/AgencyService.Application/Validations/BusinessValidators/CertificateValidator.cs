using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Validations.BusinessValidators
{
    public class CertificateValidator : ICertificateValidator
    {
        private readonly ILogger<CertificateValidator> _logger;
        private readonly ICertificateRepository _certificateRepository;

        public CertificateValidator(
            ILogger<CertificateValidator> logger,
            ICertificateRepository certificateRepository)
        {
            _logger = logger;
            _certificateRepository = certificateRepository;
        }

        public void ValidateCertificateIsNotNull(Certificate certificate)
        {
            if (certificate == null)
            {
                _logger.LogInformation("Certificate not found");
                throw new Exception("Certificate not found");
            }

            _logger.LogDebug("Validated certificate with id {CertificateId} exists", certificate?.Id);
        }

        public async Task ValidateCertificateWithIdExists(int certificateId)
        {
            var certificate = await _certificateRepository.GetByIdAsync(certificateId);
            ValidateCertificateIsNotNull(certificate);
        }
    }
}
