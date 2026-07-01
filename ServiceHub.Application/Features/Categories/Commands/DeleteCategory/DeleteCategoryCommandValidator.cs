using FluentValidation;

namespace ServiceHub.Application.Features.Categories.Commands.DeleteCategory;

/// <summary>
/// Validator rules for <see cref="DeleteCategoryCommand"/>.
/// </summary>
public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCategoryCommandValidator"/> class.
    /// </summary>
    public DeleteCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
