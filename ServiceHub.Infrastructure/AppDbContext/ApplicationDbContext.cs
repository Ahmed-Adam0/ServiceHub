using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Domain.Common;
using ServiceHub.Domain.Common.Base;
using ServiceHub.Domain.Common.Interfaces;
using ServiceHub.Application.Common.Interfaces;
using ServiceHub.Domain.Entities;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Infrastructure.AppDbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
    public DbSet<VendorProfile> VendorProfiles => Set<VendorProfile>();
    public DbSet<VendorVerification> VendorVerifications => Set<VendorVerification>();
    public DbSet<VendorAvailability> VendorAvailabilities => Set<VendorAvailability>();
    public DbSet<VendorPortfolio> VendorPortfolios => Set<VendorPortfolio>();

    // Explicit interface implementations to satisfy IApplicationDbContext without EF Core references
    IQueryable<RefreshToken> IApplicationDbContext.RefreshTokens => RefreshTokens;
    IQueryable<Category> IApplicationDbContext.Categories => Categories;
    IQueryable<CustomerProfile> IApplicationDbContext.CustomerProfiles => CustomerProfiles;
    IQueryable<VendorProfile> IApplicationDbContext.VendorProfiles => VendorProfiles;
    IQueryable<VendorVerification> IApplicationDbContext.VendorVerifications => VendorVerifications;
    IQueryable<VendorAvailability> IApplicationDbContext.VendorAvailabilities => VendorAvailabilities;
    IQueryable<VendorPortfolio> IApplicationDbContext.VendorPortfolios => VendorPortfolios;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public async Task AddCustomerProfileAsync(CustomerProfile profile, CancellationToken cancellationToken)
    {
        await CustomerProfiles.AddAsync(profile, cancellationToken);
    }

    public async Task AddVendorProfileAsync(VendorProfile profile, CancellationToken cancellationToken)
    {
        await VendorProfiles.AddAsync(profile, cancellationToken);
    }

    public async Task<CustomerProfile?> GetCustomerProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await CustomerProfiles.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);
    }

    public async Task<VendorProfile?> GetVendorProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await VendorProfiles.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply entity configurations automatically from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global query filter for soft delete
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
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

            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditable.LastModifiedAt = now;
                }
            }

            if (entry.Entity is ISoftDelete softDelete)
            {
                if (entry.State == EntityState.Added)
                {
                    softDelete.IsDeleted = false;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // Intercept delete
                    softDelete.IsDeleted = true;
                    softDelete.DeletedAt = now;
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

