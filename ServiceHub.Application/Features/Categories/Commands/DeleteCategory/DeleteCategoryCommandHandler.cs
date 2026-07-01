using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.IRepositories;

namespace ServiceHub.Application.Features.Categories.Commands.DeleteCategory;

/// <summary>
/// Handler for executing the logic of <see cref="DeleteCategoryCommand"/>.
/// </summary>
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository interface.</param>
    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch category
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure(new Error(
                "Category.NotFound",
                $"Category with ID '{request.Id}' was not found.",
                ErrorType.NotFound));
        }

        // 2. Perform deletion (The ApplicationDbContext interceptor will translate this to a soft-delete)
        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
