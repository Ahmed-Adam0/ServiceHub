using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;
using ServiceHub.Application.IRepositories;

namespace ServiceHub.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// Handler for executing the logic of <see cref="GetCategoryByIdQuery"/>.
/// </summary>
public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCategoryByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository interface.</param>
    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch category
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null || category.IsDeleted)
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.NotFound",
                $"Category with ID '{request.Id}' was not found.",
                ErrorType.NotFound));
        }

        // 2. Map to DTO
        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            Icon = category.Icon,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId
        };

        return Result.Success(dto);
    }
}
