using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="VendorProfile"/> entity.
/// </summary>
public class VendorProfileConfiguration : IEntityTypeConfiguration<VendorProfile>
{
    public void Configure(EntityTypeBuilder<VendorProfile> builder)
    {
        // Table Mapping
        builder.ToTable("VendorProfiles");

        // Primary Key Configuration
        builder.HasKey(v => v.Id);

        // Property Constraints
        builder.Property(v => v.BusinessName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Bio)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(v => v.Logo)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(v => v.CoverImage)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(v => v.AverageRating)
            .HasPrecision(3, 2)
            .HasDefaultValue(0.00)
            .IsRequired();

        builder.Property(v => v.CompletedOrders)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(v => v.IsVerified)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(v => v.Status)
            .HasConversion<int>()
            .IsRequired();

        // Audit Trail Auditable Entity Properties Mapped as Required
        builder.Property(v => v.CreatedAt)
            .IsRequired();

        // 1-to-1 Relationship with ApplicationUser
        builder.HasOne(v => v.User)
            .WithOne(u => u.VendorProfile)
            .HasForeignKey<VendorProfile>(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique Index on UserId
        builder.HasIndex(v => v.UserId)
            .IsUnique();

        // 1-to-Many Relationship with VendorAvailability
        builder.HasMany(v => v.Availabilities)
            .WithOne(a => a.VendorProfile)
            .HasForeignKey(a => a.VendorProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1-to-Many Relationship with VendorPortfolio
        builder.HasMany(v => v.PortfolioItems)
            .WithOne(p => p.VendorProfile)
            .HasForeignKey(p => p.VendorProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
