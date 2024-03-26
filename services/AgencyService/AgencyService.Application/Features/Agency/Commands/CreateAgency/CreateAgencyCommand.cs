
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using MediatR;

namespace AgencyService.Application.Features.Agency.Commands.CreateAgency;

public class CreateAgencyCommand : IRequest<AgencyResponse>
{
    public string? AgencyName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? AgencyCode { get; set; }

    public bool IsActive { get; set; } = true;
}
