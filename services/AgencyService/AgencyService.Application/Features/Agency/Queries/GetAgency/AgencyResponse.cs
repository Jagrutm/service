
namespace AgencyService.Application.Features.Agency.Queries.GetAgency;

public class AgencyResponse
{
    public Guid AgencyId { get; set; }

    public string? AgencyName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? AgencyCode { get; set; }

    public bool IsActive { get; set; } = true;
}
