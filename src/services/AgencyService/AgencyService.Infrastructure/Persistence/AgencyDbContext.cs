using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using BuildingBlocks.Core.Domain.Entities;
using BuildingBlocks.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AgencyService.Infrastructure.Persistence;

public class AgencyDbContext : EntityFrameworkDbContext, IAgencyDbContext
{
   
    public AgencyDbContext(
        DbContextOptions<AgencyDbContext> options)
        : base(options)
    {
    }

    public AgencyDbContext(string connectionString)
            : base(connectionString)
    {
    }

    public DbSet<Agency> Agencies => Set<Agency>();

    public DbSet<SortCode> SortCodes => Set<SortCode>();

    public DbSet<Certificate> Certificates => Set<Certificate>();

    public DbSet<Maintenance> Maintenances => Set<Maintenance>();

    public DbSet<Webhook> Webhooks => Set<Webhook>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<Certificate>(_ =>
        {
            _.HasOne<Agency>()
            .WithMany(x => x.Certificates)
            .HasForeignKey(x => x.AgencyId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SortCode>(_ =>
        {
            _.HasOne<Agency>()
            .WithMany(x => x.SortCodes)
            .HasForeignKey(x => x.AgencyId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Maintenance>(_ =>
        {
            _.HasOne<Agency>()
            .WithMany(x => x.Maintenances)
            .HasForeignKey(x => x.AgencyId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Webhook>(_ =>
         _.HasOne<Agency>()
         .WithMany(_ => _.Webhooks)
         .HasForeignKey(_ => _.AgencyId).OnDelete(DeleteBehavior. Cascade)
        );

        modelBuilder.Entity<Webhook>(_ =>
        _.HasIndex(e => new { e.AgencyId, e.Type }).IsUnique());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this);

        foreach (var entry in ChangeTracker.Entries<BaseAuditEntity<int>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.CreatedBy = "swn";     //TODO: this need to change 
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.Now;
                    entry.Entity.LastModifiedBy = "swn";   //TODO: this need to change 
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
