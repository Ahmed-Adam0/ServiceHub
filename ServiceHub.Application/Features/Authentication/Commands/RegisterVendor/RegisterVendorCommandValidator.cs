using FluentValidation;

namespace ServiceHub.Application.Features.Authentication.Commands.RegisterVendor;

/// <summary>
/// Validator rules for <see cref="RegisterVendorCommand"/>.
/// </summary>
public class RegisterVendorCommandValidator : AbstractValidator<RegisterVendorCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterVendorCommandValidator"/> class.
    /// </summary>
    public RegisterVendorCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not a valid email address.");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

        RuleFor(c => c.BusinessName)
            .NotEmpty().WithMessage("Business name is required.")
            .MaximumLength(100).WithMessage("Business name cannot exceed 100 characters.");

        RuleFor(c => c.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(100).WithMessage("Display name cannot exceed 100 characters.");

        RuleFor(c => c.Bio)
            .NotEmpty().WithMessage("Bio description is required.")
            .MaximumLength(1000).WithMessage("Bio description cannot exceed 1000 characters.");
    }
}
