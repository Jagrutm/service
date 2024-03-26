using AgencyService.Domain.Entities;

namespace AgencyService.Application.Contracts.Validations
{
    public interface ICertificateValidator
    {
        Task ValidateCertificateWithIdExists(int certificateId);

        void ValidateCertificateIsNotNull(Certificate certificate);
    }
}