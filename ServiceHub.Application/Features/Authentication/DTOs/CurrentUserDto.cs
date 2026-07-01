using System;

namespace ServiceHub.Application.Features.Authentication.DTOs;

/// <summary>
/// Data Transfer Object representing the authenticated user context and profile details.
/// </summary>
public class CurrentUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Role { get; set; }
    public CustomerProfileDto? CustomerProfile { get; set; }
    public VendorProfileDto? VendorProfile { get; set; }
}

/// <summary>
/// Data Transfer Object representing customer profile details.
/// </summary>
public class CustomerProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfileImage { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

/// <summary>
/// Data Transfer Object representing vendor profile details.
/// </summary>
public class VendorProfileDto
{
    public Guid Id { get; set; }
    public string BusinessName { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string? Logo { get; set; }
    public string? CoverImage { get; set; }
    public decimal AverageRating { get; set; }
    public int CompletedOrders { get; set; }
    public bool IsVerified { get; set; }
    public string Status { get; set; } = null!;
}
