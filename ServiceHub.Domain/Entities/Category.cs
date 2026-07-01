using System;
using System.Collections.Generic;
using ServiceHub.Domain.Common.Base;

namespace ServiceHub.Domain.Entities;

/// <summary>
/// Represents a hierarchical taxonomy classification category for marketplace service listings.
/// </summary>
public class Category : SoftDeleteEntity
{
    private readonly List<Category> _childCategories = new();

    /// <summary>
    /// Gets the category name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets the unique URL slug.
    /// </summary>
    public string Slug { get; private set; } = null!;

    /// <summary>
    /// Gets the category description.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets the visual icon styling class or link.
    /// </summary>
    public string Icon { get; private set; } = null!;

    /// <summary>
    /// Gets the order in which the category is sorted on the catalog screen.
    /// </summary>
    public int DisplayOrder { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the category is active and visible in the marketplace.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the identifier of the parent category, if any.
    /// </summary>
    public Guid? ParentCategoryId { get; private set; }

    // Navigation properties

    /// <summary>
    /// Gets the parent category.
    /// </summary>
    public virtual Category? ParentCategory { get; private set; }

    /// <summary>
    /// Gets the read-only collection of subcategories.
    /// </summary>
    public virtual IReadOnlyCollection<Category> ChildCategories => _childCategories.AsReadOnly();

    // Parameterless constructor for EF Core materialization
    private Category()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="name">The name of the category.</param>
    /// <param name="slug">The URL slug.</param>
    /// <param name="description">The details description.</param>
    /// <param name="icon">The visual icon class.</param>
    /// <param name="displayOrder">The sorting sort index.</param>
    /// <param name="parentCategory">The optional parent category reference.</param>
    public Category(
        string name,
        string slug,
        string description,
        string icon,
        int displayOrder,
        Category? parentCategory = null)
    {
        UpdateDetails(name, slug, description, icon, displayOrder);
        SetParentCategory(parentCategory);
        IsActive = true;
    }

    /// <summary>
    /// Updates the main details of the category.
    /// </summary>
    public void UpdateDetails(string name, string slug, string description, string icon, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Category slug cannot be empty.", nameof(slug));

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = description?.Trim() ?? string.Empty;
        Icon = icon?.Trim() ?? string.Empty;
        DisplayOrder = displayOrder;
    }

    /// <summary>
    /// Sets or clears the parent category, enforcing business validation rules.
    /// </summary>
    /// <param name="parentCategory">The parent category.</param>
    public void SetParentCategory(Category? parentCategory)
    {
        if (parentCategory != null)
        {
            if (parentCategory.Id == Id && Id != Guid.Empty)
            {
                throw new InvalidOperationException("A category cannot be its own parent.");
            }
            if (parentCategory.IsDeleted)
            {
                throw new InvalidOperationException("A soft-deleted category cannot be set as a parent.");
            }
        }

        ParentCategory = parentCategory;
        ParentCategoryId = parentCategory?.Id;
    }

    /// <summary>
    /// Activates the category.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the category.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
