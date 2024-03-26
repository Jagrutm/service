
using AgencyService.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Features.Agency.Queries.GetAgency;

public class GetAgencyQueryHandler : IRequestHandler<GetAgencyQuery, AgencyResponse>
{
    private readonly IAgencyRepository _agencyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAgencyQueryHandler> _logger;

    public GetAgencyQueryHandler(IAgencyRepository agencyRepository, IMapper mapper, ILogger<GetAgencyQueryHandler> logger)
    {
        _agencyRepository = agencyRepository ?? throw new ArgumentNullException(nameof(agencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AgencyResponse> Handle(
        GetAgencyQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search by AgencyID: {0}", request.AgencyId);

        var agencyList = await _agencyRepository.GetAgencyByUIdAsync(request.AgencyId);
        return _mapper.Map<AgencyResponse>(agencyList);
    }
}
