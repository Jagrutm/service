using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyService.Infrastructure.Configuration.Entity;

public class AgencyConfiguration : IEntityTypeConfiguration<Agency>
{
    public void Configure(EntityTypeBuilder<Agency> builder)
    {
        builder.Property(_ => _.Id)
            .IsRequired(true);

        builder.Property(_ => _.Name)
           .IsRequired()
           .HasMaxLength(255);

        builder.Property(_ => _.RegistrationDate)
            .IsRequired(true);

        builder.Property(_ => _.Code)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(_ => _.IsActive)
            .HasDefaultValue(true);
    }
}
