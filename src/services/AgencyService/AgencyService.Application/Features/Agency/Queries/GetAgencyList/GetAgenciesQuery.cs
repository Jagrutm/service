
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using MediatR;

namespace AgencyService.Application.Features.Agency.Queries.GetAgencyList;

public class GetAgenciesQuery : IRequest<List<AgencyResponse>>
{
}
