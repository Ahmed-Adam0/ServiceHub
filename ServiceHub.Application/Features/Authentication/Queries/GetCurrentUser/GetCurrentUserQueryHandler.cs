using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ServiceHub.Application.Common.Interfaces;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Authentication.DTOs;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Application.Features.Authentication.Queries.GetCurrentUser;

/// <summary>
/// Handler for executing the logic of <see cref="GetCurrentUserQuery"/>.
/// </summary>
public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCurrentUserQueryHandler"/> class.
    /// </summary>
    public GetCurrentUserQueryHandler(
        UserManager<ApplicationUser> userManager,
        IApplicationDbContext context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Result<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch basic Identity user details
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return Result.Failure<CurrentUserDto>(new Error(
                "User.NotFound",
                $"User with ID '{request.UserId}' was not found.",
                ErrorType.NotFound));
        }

        // 2. Fetch associated roles
        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault();

        // 3. Build profile details using isolated IApplicationDbContext lookups
        CustomerProfileDto? customerProfileDto = null;
        VendorProfileDto? vendorProfileDto = null;

        if (primaryRole == "Customer")
        {
            var customerProfile = await _context.GetCustomerProfileByUserIdAsync(user.Id, cancellationToken);
            if (customerProfile != null)
            {
                customerProfileDto = new CustomerProfileDto
                {
                    Id = customerProfile.Id,
                    FirstName = customerProfile.FirstName,
                    LastName = customerProfile.LastName,
                    ProfileImage = customerProfile.ProfileImage,
                    PhoneNumber = customerProfile.PhoneNumber,
                    Address = customerProfile.Address,
                    DateOfBirth = customerProfile.DateOfBirth
                };
            }
        }
        else if (primaryRole == "Vendor")
        {
            var vendorProfile = await _context.GetVendorProfileByUserIdAsync(user.Id, cancellationToken);
            if (vendorProfile != null)
            {
                vendorProfileDto = new VendorProfileDto
                {
                    Id = vendorProfile.Id,
                    BusinessName = vendorProfile.BusinessName,
                    DisplayName = vendorProfile.DisplayName,
                    Bio = vendorProfile.Bio,
                    Logo = vendorProfile.Logo,
                    CoverImage = vendorProfile.CoverImage,
                    AverageRating = vendorProfile.AverageRating,
                    CompletedOrders = vendorProfile.CompletedOrders,
                    IsVerified = vendorProfile.IsVerified,
                    Status = vendorProfile.Status.ToString()
                };
            }
        }

        // 4. Map full context to CurrentUserDto
        var currentUserDto = new CurrentUserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Role = primaryRole,
            CustomerProfile = customerProfileDto,
            VendorProfile = vendorProfileDto
        };

        return Result.Success(currentUserDto);
    }
}
