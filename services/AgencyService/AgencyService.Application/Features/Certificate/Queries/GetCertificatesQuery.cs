using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AutoMapper;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Queries
{
    public class GetCertificatesQuery : IRequest<List<CertificateResponse>>
    {
        public Guid AgencyId { get; set; }
    }

    public class GetCertificatesQueryHandler : IRequestHandler<GetCertificatesQuery, List<CertificateResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAgencyValidator _agencyValidator;
        private readonly ICertificateRepository _certificateRepository;

        public GetCertificatesQueryHandler(
            IMapper mapper,
            IAgencyValidator agencyValidator,
            ICertificateRepository certificateRepository)
        {
            _mapper = mapper;
            _agencyValidator = agencyValidator;
            _certificateRepository = certificateRepository;
        }

        public async Task<List<CertificateResponse>> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
        {
            var agency = await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);
            var existingCertificates = await _certificateRepository.GetCertificatesForAgency(agency.Id);
            return _mapper.Map<List<CertificateResponse>>(existingCertificates);
        }
    }
}
