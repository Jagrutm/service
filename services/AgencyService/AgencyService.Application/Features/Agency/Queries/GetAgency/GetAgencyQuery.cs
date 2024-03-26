
using MediatR;

namespace AgencyService.Application.Features.Agency.Queries.GetAgency;

public class GetAgencyQuery : IRequest<AgencyResponse>
{
    public Guid AgencyId { get; set; }
}
