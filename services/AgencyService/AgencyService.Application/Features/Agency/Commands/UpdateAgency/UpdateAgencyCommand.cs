
using MediatR;

namespace AgencyService.Application.Features.Agency.Commands.UpdateAgency;

public class UpdateAgencyCommand : IRequest
{
    public Guid AgencyId { get; set; }

    public string? AgencyName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? AgencyCode { get; set; }

    public bool IsActive { get; set; } = true;

}
