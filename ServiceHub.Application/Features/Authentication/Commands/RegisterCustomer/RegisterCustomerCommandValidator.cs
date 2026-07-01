using FluentValidation;

namespace ServiceHub.Application.Features.Authentication.Commands.RegisterCustomer;

/// <summary>
/// Validator rules for <see cref="RegisterCustomerCommand"/>.
/// </summary>
public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCustomerCommandValidator"/> class.
    /// </summary>
    public RegisterCustomerCommandValidator()
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

        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}
