using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;
using ServiceHub.Application.IRepositories;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Handler for executing the logic of <see cref="CreateCategoryCommand"/>.
/// </summary>
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository interface.</param>
    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify Category Name uniqueness
        if (!await _categoryRepository.IsNameUniqueAsync(request.Name, null, cancellationToken))
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.DuplicateName",
                $"Category with name '{request.Name}' already exists.",
                ErrorType.Conflict));
        }

        // 2. Verify Category Slug uniqueness
        if (!await _categoryRepository.IsSlugUniqueAsync(request.Slug, null, cancellationToken))
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.DuplicateSlug",
                $"Category with slug '{request.Slug}' already exists.",
                ErrorType.Conflict));
        }

        // 3. Verify Parent Category, if provided
        Category? parentCategory = null;
        if (request.ParentCategoryId.HasValue)
        {
            parentCategory = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
            {
                return Result.Failure<CategoryDto>(new Error(
                    "Category.ParentNotFound",
                    "The specified parent category does not exist.",
                    ErrorType.NotFound));
            }
        }

        // 4. Instantiate Category Entity
        var category = new Category(
            request.Name,
            request.Slug,
            request.Description,
            request.Icon,
            request.DisplayOrder,
            parentCategory);

        // 5. Add and commit to repository
        await _categoryRepository.AddAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        // 6. Map to output DTO
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
