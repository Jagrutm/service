using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Features.Agency.Commands.CreateAgency;

public class CreateAgencyCommandHandler : IRequestHandler<CreateAgencyCommand, AgencyResponse>
{
    private readonly IAgencyRepository _agencyRepository;
    private readonly IMapper _mapper;
    //private readonly ILogger<CreateAgencyCommandHandler> _logger;

    public CreateAgencyCommandHandler(
        IAgencyRepository agencyRepository,
        IMapper mapper)
        //ILogger<CreateAgencyCommandHandler> logger)
    {
        _agencyRepository = agencyRepository ?? throw new ArgumentNullException(nameof(agencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AgencyResponse> Handle(
        CreateAgencyCommand request,
        CancellationToken cancellationToken)
    {
        var agencyEntity = _mapper.Map<Domain.Entities.Agency>(request);
        agencyEntity.UId = Guid.NewGuid();
        var newAgency = await _agencyRepository.CreateAsync(agencyEntity);

        //_logger.LogInformation($"Agency {newAgency.UId} is successfully created.");
        
        var createdAgency = _mapper.Map<AgencyResponse>(request);
        createdAgency.AgencyId = agencyEntity.UId;
        return createdAgency;
    }
}
