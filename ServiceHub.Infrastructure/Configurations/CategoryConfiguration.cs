using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Infrastructure.Configurations;

/// <summary>
/// Database configuration mappings for the <see cref="Category"/> entity.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table Mapping
        builder.ToTable("Categories");

        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        // Property Configurations
        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.Icon)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.DisplayOrder)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // Audit Trail Auditable Entity Properties Mapped as Required
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // 1-to-Many Self-Referencing Parent Category Relationship
        builder.HasOne(c => c.ParentCategory)
            .WithMany(p => p.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes of child categories

        // Unique Indexes for Search Fields
        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasIndex(c => c.Slug)
            .IsUnique();
    }
}
