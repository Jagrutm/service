using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyService.Infrastructure.Configuration.Entity
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.Property(_ => _.Id)
                .IsRequired();

            builder.Property(_ => _.AgencyId)
               .IsRequired()
               .HasMaxLength(4);

            builder.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(_ => _.Key)
                .IsRequired()
               .HasMaxLength(4000);

            builder.Property(_ => _.StartDate)
                .IsRequired();

            builder.Property(_ => _.ExpiryDate)
                .IsRequired();

            builder.Property(_ => _.IsActive)
                .IsRequired();

            builder.Property(_ => _.Type)
                .IsRequired();
        }
    }
}
