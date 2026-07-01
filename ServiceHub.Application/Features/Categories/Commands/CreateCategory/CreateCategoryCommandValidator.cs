using FluentValidation;

namespace ServiceHub.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Validator rules for <see cref="CreateCategoryCommand"/>.
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCategoryCommandValidator"/> class.
    /// </summary>
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

        RuleFor(c => c.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug cannot exceed 100 characters.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric characters and hyphens only.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(c => c.Icon)
            .NotEmpty().WithMessage("Icon identifier is required.")
            .MaximumLength(100).WithMessage("Icon identifier cannot exceed 100 characters.");

        RuleFor(c => c.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
    }
}
