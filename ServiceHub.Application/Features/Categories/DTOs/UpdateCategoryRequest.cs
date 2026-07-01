using System;

namespace ServiceHub.Application.Features.Categories.DTOs;

/// <summary>
/// Input request details for updating an existing category.
/// </summary>
public class UpdateCategoryRequest
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
