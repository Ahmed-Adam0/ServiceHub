using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Domain.Common;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Infrastructure.AppDbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply entity configurations automatically from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global query filter for soft delete
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity<Guid>).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(ConfigureSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                
                method?.Invoke(null, new object[] { builder });
            }
            else if (entityType.ClrType == typeof(ApplicationUser))
            {
                builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
            }
        }
    }

    private static void ConfigureSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : class
    {
        builder.Entity<TEntity>().HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
    }

    public override int SaveChanges()
    {
        ApplyAuditingAndSoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditingAndSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditingAndSoftDelete()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;

            if (entry.Entity is BaseEntity<Guid> entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // Intercept delete
                    entity.IsDeleted = true;
                    entity.DeletedAt = now;
                }
            }
            else if (entry.Entity is ApplicationUser user)
            {
                if (entry.State == EntityState.Added)
                {
                    user.CreatedAt = now;
                    user.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    user.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // Intercept delete
                    user.IsDeleted = true;
                    user.DeletedAt = now;
                }
            }
        }
    }
}

