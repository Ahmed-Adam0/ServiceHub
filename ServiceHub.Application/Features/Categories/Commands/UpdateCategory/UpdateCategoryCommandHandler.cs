using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;
using ServiceHub.Application.IRepositories;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// Handler for executing the logic of <see cref="UpdateCategoryCommand"/>.
/// </summary>
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository interface.</param>
    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch existing category
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.NotFound",
                $"Category with ID '{request.Id}' was not found.",
                ErrorType.NotFound));
        }

        // 2. Verify Category Name uniqueness (excluding the current category)
        if (!await _categoryRepository.IsNameUniqueAsync(request.Name, category.Id, cancellationToken))
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.DuplicateName",
                $"Category with name '{request.Name}' already exists.",
                ErrorType.Conflict));
        }

        // 3. Verify Category Slug uniqueness (excluding the current category)
        if (!await _categoryRepository.IsSlugUniqueAsync(request.Slug, category.Id, cancellationToken))
        {
            return Result.Failure<CategoryDto>(new Error(
                "Category.DuplicateSlug",
                $"Category with slug '{request.Slug}' already exists.",
                ErrorType.Conflict));
        }

        // 4. Verify Parent Category, if provided
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

        try
        {
            // 5. Mutate state via domain model business methods
            category.UpdateDetails(
                request.Name,
                request.Slug,
                request.Description,
                request.Icon,
                request.DisplayOrder);

            category.SetParentCategory(parentCategory);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<CategoryDto>(new Error("Category.InvalidParent", ex.Message, ErrorType.Validation));
        }

        // 6. Save changes
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        // 7. Map to DTO
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
