using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AgencyService.Application.Features.Certificate.Commands
{
    public class CreateCertificateCommand : IRequest<int>
    {
        public Guid AgencyId { get; set; }

        public string CertificateName { get; set; }

        public string CertificateKey { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public CertificateType Type { get; set; } = CertificateType.Agency;
    }

    public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IAgencyValidator _agencyValidator;

        public CreateCertificateCommandHandler(
            IMapper mapper, 
            ICertificateRepository certificateRepository,
            IAgencyValidator agencyValidator)
        {
            _mapper = mapper;
            _certificateRepository = certificateRepository;
            _agencyValidator = agencyValidator;
        }

        public async Task<int> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
        {
            var agency = await _agencyValidator.ValidateAgencyWithIdExists(request.AgencyId);

            var certificateToCreate = _mapper.Map<Domain.Entities.Certificate>(request);
            certificateToCreate.AgencyId = agency.Id;
            await _certificateRepository.CreateAsync(certificateToCreate);
            return certificateToCreate.Id;
        }
    }

}
