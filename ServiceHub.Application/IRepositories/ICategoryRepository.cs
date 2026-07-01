using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Application.IRepositories;

/// <summary>
/// Defines persistence operations for the <see cref="Category"/> aggregate root.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Gets a category by its unique identifier.
    /// </summary>
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a category by its slug parameter.
    /// </summary>
    Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies if a category name is unique, excluding a specified category identifier.
    /// </summary>
    Task<bool> IsNameUniqueAsync(string name, Guid? excludeId, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies if a category slug is unique, excluding a specified category identifier.
    /// </summary>
    Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all active, non-deleted categories.
    /// </summary>
    Task<IReadOnlyList<Category>> GetAllActiveAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new category.
    /// </summary>
    Task AddAsync(Category category, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing category details.
    /// </summary>
    void Update(Category category);

    /// <summary>
    /// Deletes (or logically marks deleted) a category.
    /// </summary>
    void Delete(Category category);

    /// <summary>
    /// Commits transaction operations to database storage.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
