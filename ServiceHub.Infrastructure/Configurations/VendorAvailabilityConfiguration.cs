using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="VendorAvailability"/> entity.
/// </summary>
public class VendorAvailabilityConfiguration : IEntityTypeConfiguration<VendorAvailability>
{
    public void Configure(EntityTypeBuilder<VendorAvailability> builder)
    {
        // Table Mapping
        builder.ToTable("VendorAvailabilities");

        // Primary Key Configuration
        builder.HasKey(a => a.Id);

        // Property Constraints
        builder.Property(a => a.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.StartTime)
            .IsRequired();

        builder.Property(a => a.EndTime)
            .IsRequired();

        builder.Property(a => a.IsAvailable)
            .HasDefaultValue(true)
            .IsRequired();
    }
}
