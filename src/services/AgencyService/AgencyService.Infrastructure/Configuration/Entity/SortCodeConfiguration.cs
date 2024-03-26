using AgencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyService.Infrastructure.Configuration.Entity
{
    public class SortCodeConfiguration : IEntityTypeConfiguration<SortCode>
    {
        public void Configure(EntityTypeBuilder<SortCode> builder)
        {
            builder.Property(_ => _.Id)
                .IsRequired();

            builder.Property(_ => _.Value)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(_ => _.DisplaySortCode)
               .HasMaxLength(100);

            builder.Property(_ => _.AccountNumberSize)
                .IsRequired();

            builder.Property(_ => _.ChecksumLogic)
                .IsRequired();

            builder.Property(_ => _.Weightage)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(_ => _.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

        }

        //public void Configure(ModelBuilder builder)
        //{

        //    builder.Entity<SortCode>(_ =>
        //    {
        //        _.ToTable("JobApplications");
        //        _.HasKey(e => e.Id);
        //        _.Property(e => e.Id).HasColumnType("Int").IsRequired();
        //        _.Property(_ => _.Value).IsRequired().HasMaxLength(20);
        //        _.Property(_ => _.DisplaySortCode).IsRequired().HasMaxLength(100);
        //        _.Property(_ => _.AccountNumberSize).IsRequired();
        //        _.Property(_ => _.ChecksumLogic).IsRequired();
        //        _.Property(_ => _.Weightage).IsRequired().HasMaxLength(100);
        //        _.Property(e => e.IsActive).HasDefaultValue(true).IsRequired();
        //    });
        //}
    }
}
