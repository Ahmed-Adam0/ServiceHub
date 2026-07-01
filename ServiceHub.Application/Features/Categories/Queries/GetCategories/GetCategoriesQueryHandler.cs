using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;
using ServiceHub.Application.IRepositories;

namespace ServiceHub.Application.Features.Categories.Queries.GetCategories;

/// <summary>
/// Handler for executing the logic of <see cref="GetCategoriesQuery"/>.
/// </summary>
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCategoriesQueryHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository interface.</param>
    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch active categories
        var categories = await _categoryRepository.GetAllActiveAsync(cancellationToken);

        // 2. Map to DTOs list
        var dtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Slug = c.Slug,
            Description = c.Description,
            Icon = c.Icon,
            DisplayOrder = c.DisplayOrder,
            IsActive = c.IsActive,
            ParentCategoryId = c.ParentCategoryId
        }).ToList().AsReadOnly();

        return Result.Success<IReadOnlyList<CategoryDto>>(dtos);
    }
}
