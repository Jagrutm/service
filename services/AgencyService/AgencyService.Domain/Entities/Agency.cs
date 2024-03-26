using BuildingBlocks.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgencyService.Domain.Entities;

public class Agency : BaseAuditEntity<int>
{
    public Guid UId { get; set; }

    public string? Name { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; } = true;

    public long MinAccountBalance { get; set; }

    public List<Webhook> Webhooks { get; set; }

    public List<Certificate> Certificates { get; set; }

    public List<SortCode> SortCodes { get; set; }

    public List<Maintenance> Maintenances { get; set; }

}
