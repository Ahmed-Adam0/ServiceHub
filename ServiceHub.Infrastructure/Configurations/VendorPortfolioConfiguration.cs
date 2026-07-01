using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="VendorPortfolio"/> entity.
/// </summary>
public class VendorPortfolioConfiguration : IEntityTypeConfiguration<VendorPortfolio>
{
    public void Configure(EntityTypeBuilder<VendorPortfolio> builder)
    {
        // Table Mapping
        builder.ToTable("VendorPortfolios");

        // Primary Key Configuration
        builder.HasKey(p => p.Id);

        // Property Constraints
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.DisplayOrder)
            .HasDefaultValue(0)
            .IsRequired();

        // Audit Trail Auditable Entity Properties Mapped as Required
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Composite Unique Index: DisplayOrder must be unique per Vendor Profile
        builder.HasIndex(p => new { p.VendorProfileId, p.DisplayOrder })
            .IsUnique();
    }
}
