using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgencyService.Application.Contracts.Persistence;

public interface IAgencyDbContext
{

    DbSet<Agency> Agencies { get; }

    DbSet<SortCode> SortCodes { get; }

    DbSet<Certificate> Certificates { get; }

    DbSet<Maintenance> Maintenances { get; }

    DbSet<Webhook> Webhooks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
