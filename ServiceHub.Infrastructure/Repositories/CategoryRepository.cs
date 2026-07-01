using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Application.IRepositories;
using ServiceHub.Domain.Entities;
using ServiceHub.Infrastructure.AppDbContext;

namespace ServiceHub.Infrastructure.Repositories;

/// <summary>
/// Infrastructure implementation of persistence operations for the <see cref="Category"/> entity.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Slug == slug && !c.IsDeleted, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId, CancellationToken cancellationToken)
    {
        var normalizedName = name.Trim().ToLower();
        return !await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == normalizedName 
                           && (!excludeId.HasValue || c.Id != excludeId.Value) 
                           && !c.IsDeleted, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId, CancellationToken cancellationToken)
    {
        var normalizedSlug = slug.Trim().ToLower();
        return !await _context.Categories
            .AnyAsync(c => c.Slug.ToLower() == normalizedSlug 
                           && (!excludeId.HasValue || c.Id != excludeId.Value) 
                           && !c.IsDeleted, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Category>> GetAllActiveAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    /// <inheritdoc />
    public void Delete(Category category)
    {
        // Entity removal state is intercepted by DbContext and converted to soft-delete
        _context.Categories.Remove(category);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
