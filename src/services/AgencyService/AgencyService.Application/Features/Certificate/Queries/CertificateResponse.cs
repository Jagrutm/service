using AgencyService.Application.Enums;
using AgencyService.Domain.Enums;

namespace AgencyService.Application.Features.Certificate.Queries
{
    public class CertificateResponse
    {
        public Guid AgencyId { get; set; }

        public string CertificateName { get; set; }

        public string CertificateKey { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Type { get; set; }
    }
}
