using System;
using ServiceHub.Domain.Common.Base;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Domain.Identity.Entities;

/// <summary>
/// Represents all marketplace-related business information for a Customer.
/// </summary>
public class CustomerProfile : SoftDeleteEntity
{
    /// <summary>
    /// Gets the unique identifier of the linked Identity user account.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the customer's first name.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Gets the customer's last name.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Gets the customer's profile image file path or URL.
    /// </summary>
    public string? ProfileImage { get; private set; }

    /// <summary>
    /// Gets the customer's business contact phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets the customer's mailing/billing address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Gets the customer's date of birth.
    /// </summary>
    public DateTime? DateOfBirth { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the customer profile is active.
    /// </summary>
    public bool IsActive { get; private set; }

    // Navigation property

    /// <summary>
    /// Gets the linked application user.
    /// </summary>
    public virtual ApplicationUser User { get; private set; } = null!;

    // Parameterless constructor for EF Core materialization
    private CustomerProfile()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerProfile"/> class.
    /// </summary>
    public CustomerProfile(
        Guid userId,
        string firstName,
        string lastName,
        string? profileImage = null,
        string? phoneNumber = null,
        string? address = null,
        DateTime? dateOfBirth = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        UserId = userId;
        UpdateProfile(firstName, lastName, profileImage, phoneNumber, address, dateOfBirth);
        IsActive = true;
    }

    /// <summary>
    /// Updates the customer profile details.
    /// </summary>
    public void UpdateProfile(
        string firstName,
        string lastName,
        string? profileImage,
        string? phoneNumber,
        string? address,
        DateTime? dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (dateOfBirth.HasValue && dateOfBirth.Value >= DateTime.Today)
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        ProfileImage = profileImage?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        Address = address?.Trim();
        DateOfBirth = dateOfBirth;
    }

    /// <summary>
    /// Activates the customer profile.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the customer profile.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
