using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ServiceHub.Application.Common.Interfaces;
using ServiceHub.Application.Common.Models;
using ServiceHub.Domain.Entities;
using ServiceHub.Domain.Identity.Entities;

namespace ServiceHub.Application.Features.Authentication.Commands.RegisterVendor;

/// <summary>
/// Handler for executing the logic of <see cref="RegisterVendorCommand"/>.
/// </summary>
public class RegisterVendorCommandHandler : IRequestHandler<RegisterVendorCommand, Result<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterVendorCommandHandler"/> class.
    /// </summary>
    public RegisterVendorCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IApplicationDbContext context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
    {
        // 1. Check duplicate Email
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result.Failure<Guid>(new Error(
                "Auth.DuplicateEmail",
                $"Email '{request.Email}' is already registered.",
                ErrorType.Conflict));
        }

        // 2. Instantiate User Account
        var user = new ApplicationUser(request.Email, request.FullName);
        
        // Mark email confirmed as default for mock/sandbox purposes, or let standard pipeline confirm it
        user.EmailConfirmed = true;

        // 3. Create User in ASP.NET Core Identity
        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(e => e.Description);
            return Result.Failure<Guid>(new ValidationError(errors));
        }

        // 4. Ensure Vendor role is defined and assign it to the user
        const string roleName = "Vendor";
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
        await _userManager.AddToRoleAsync(user, roleName);

        // 5. Create VendorProfile entity linked to user
        var vendorProfile = new VendorProfile(
            user.Id,
            request.BusinessName,
            request.DisplayName,
            request.Bio,
            request.Logo,
            request.CoverImage);

        // 6. Save vendor profile details
        await _context.AddVendorProfileAsync(vendorProfile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
