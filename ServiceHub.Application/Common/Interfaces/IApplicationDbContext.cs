using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ServiceHub.Domain.Entities;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Application.Common.Interfaces;

/// <summary>
/// Exposes queryable database context and addition contracts for the application layer,
/// avoiding any reference dependencies on Entity Framework Core.
/// </summary>
public interface IApplicationDbContext
{
    IQueryable<RefreshToken> RefreshTokens { get; }
    IQueryable<Category> Categories { get; }
    IQueryable<CustomerProfile> CustomerProfiles { get; }
    IQueryable<VendorProfile> VendorProfiles { get; }
    IQueryable<VendorVerification> VendorVerifications { get; }
    IQueryable<VendorAvailability> VendorAvailabilities { get; }
    IQueryable<VendorPortfolio> VendorPortfolios { get; }

    /// <summary>
    /// Asynchronously schedules a new customer profile creation.
    /// </summary>
    Task AddCustomerProfileAsync(CustomerProfile profile, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously schedules a new vendor profile creation.
    /// </summary>
    Task AddVendorProfileAsync(VendorProfile profile, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves an active customer profile by user identifier.
    /// </summary>
    Task<CustomerProfile?> GetCustomerProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves an active vendor profile by user identifier.
    /// </summary>
    Task<VendorProfile?> GetVendorProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Commits all modified entities in the context to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
