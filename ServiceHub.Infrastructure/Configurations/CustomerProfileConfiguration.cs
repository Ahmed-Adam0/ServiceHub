using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="CustomerProfile"/> entity.
/// </summary>
public class CustomerProfileConfiguration : IEntityTypeConfiguration<CustomerProfile>
{
    public void Configure(EntityTypeBuilder<CustomerProfile> builder)
    {
        // Table Mapping
        builder.ToTable("CustomerProfiles");

        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        // Property Constraints
        builder.Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ProfileImage)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(c => c.Address)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(c => c.DateOfBirth)
            .IsRequired(false);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // Audit Trail Auditable Entity Properties Mapped as Required
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // 1-to-1 Relationship with ApplicationUser
        builder.HasOne(c => c.User)
            .WithOne(u => u.CustomerProfile)
            .HasForeignKey<CustomerProfile>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique Index on UserId
        builder.HasIndex(c => c.UserId)
            .IsUnique();
    }
}
