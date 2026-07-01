using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="VendorVerification"/> entity.
/// </summary>
public class VendorVerificationConfiguration : IEntityTypeConfiguration<VendorVerification>
{
    public void Configure(EntityTypeBuilder<VendorVerification> builder)
    {
        // Table Mapping
        builder.ToTable("VendorVerifications");

        // Primary Key Configuration
        builder.HasKey(vv => vv.Id);

        // Property Constraints
        builder.Property(vv => vv.NationalId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(vv => vv.DocumentPath)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(vv => vv.VerificationStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(vv => vv.VerifiedAt)
            .IsRequired(false);

        builder.Property(vv => vv.Notes)
            .HasMaxLength(500)
            .IsRequired(false);

        // Audit Trail Auditable Entity Properties Mapped as Required
        builder.Property(vv => vv.CreatedAt)
            .IsRequired();

        // 1-to-1 Relationship with VendorProfile
        builder.HasOne(vv => vv.VendorProfile)
            .WithOne(vp => vp.Verification)
            .HasForeignKey<VendorVerification>(vv => vv.VendorProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique Index on VendorProfileId
        builder.HasIndex(vv => vv.VendorProfileId)
            .IsUnique();
    }
}
