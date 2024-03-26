using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyService.Infrastructure.Configuration.Entity
{
    public class WebhookConfiguration : IEntityTypeConfiguration<Webhook>
    {
        public void Configure(EntityTypeBuilder<Webhook> builder)
        {
            builder.Property(_ => _.Id)
            .IsRequired(true);

            builder.Property(_ => _.Id)
            .IsRequired(true);

            builder.Property(_ => _.AgencyId)
                .IsRequired(true);

            builder.Property(_ => _.Type)
                .IsRequired(true);

            builder.Property(_ => _.Url)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(_ => _.CreatedDate)
                .HasDefaultValue(DateTime.Now);
        }
    }
}
