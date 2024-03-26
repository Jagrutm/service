using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyService.Infrastructure.Configuration.Entity
{
    public class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
    {
        public void Configure(EntityTypeBuilder<Maintenance> builder)
        {
            builder.Property(_ => _.Id)
                .IsRequired();

            builder.Property(_ => _.AgencyId)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(_ => _.ToDate)
                .IsRequired();

            builder.Property(_ => _.FromDate)
                .IsRequired();

            builder.Property(_ => _.ResponseCode)
               .HasMaxLength(12);
        }
    }
}
