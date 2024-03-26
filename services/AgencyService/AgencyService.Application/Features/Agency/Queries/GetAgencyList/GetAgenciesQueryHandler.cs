
using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using AutoMapper;
using MediatR;

namespace AgencyService.Application.Features.Agency.Queries.GetAgencyList;

public class GetAgenciesQueryHandler : IRequestHandler<GetAgenciesQuery, List<AgencyResponse>>
{
    private readonly IAgencyRepository _agencyRepository;
    private readonly IMapper _mapper;

    public GetAgenciesQueryHandler(IAgencyRepository agencyRepository, IMapper mapper)
    {
        _agencyRepository = agencyRepository ?? throw new ArgumentNullException(nameof(agencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<AgencyResponse>> Handle(
        GetAgenciesQuery request, 
        CancellationToken cancellationToken)
    {
        var agencyList = await _agencyRepository.GetAllAsync();
        return _mapper.Map<List<AgencyResponse>>(agencyList);
    }
}
