using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Enums;
using AutoMapper;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Queries
{
    public class GetCertificateByIdQuery : IRequest<CertificateResponse>
    {
        public Guid AgencyId { get; set; }

        public int CertificateId { get; set; } 
    }

    public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, CertificateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAgencyValidator _agencyValidator;
        private readonly ICertificateValidator _certificateValidator;
        private readonly ICertificateRepository _certificateRepository;

        public GetCertificateByIdQueryHandler(
            IMapper mapper,
            IAgencyValidator agencyValidator,
            ICertificateValidator certificateValidator,
            ICertificateRepository certificateRepository)
        {
            _mapper = mapper;
            _agencyValidator = agencyValidator;
            _certificateValidator = certificateValidator;
            _certificateRepository = certificateRepository;
        }

        public async Task<CertificateResponse> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
        {
            var agency = await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);

            var certificate = await _certificateRepository.GetByIdAsync(request.CertificateId);
            _certificateValidator.ValidateCertificateIsNotNull(certificate);

            var certificateResponse = _mapper.Map<CertificateResponse>(certificate);
            certificateResponse.AgencyId = agency.UId;
            return certificateResponse;
        }
    }
}
