using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TokenHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.Expiry)
            .IsRequired();

        builder.Property(t => t.IsRevoked)
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedByIp)
            .HasMaxLength(50);

        // One-to-many relationship with ApplicationUser
        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete tokens if user is deleted

        // Indexes for performance
        builder.HasIndex(t => t.TokenHash)
            .HasDatabaseName("IX_RefreshTokens_TokenHash");
    }
}

