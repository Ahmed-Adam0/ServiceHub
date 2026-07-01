using System;
using System.Collections.Generic;
using System.Linq;
using ServiceHub.Domain.Common.Base;
using ServiceHub.Domain.Entities;
using ServiceHub.Domain.Enums;

namespace ServiceHub.Domain.Identity.Entities;

/// <summary>
/// Represents the marketplace-related business profile of a Vendor.
/// </summary>
public class VendorProfile : SoftDeleteEntity
{
    private readonly List<VendorAvailability> _availabilities = new();
    private readonly List<VendorPortfolio> _portfolioItems = new();

    /// <summary>
    /// Gets the unique identifier of the linked Identity user account.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the vendor's trade or business name.
    /// </summary>
    public string BusinessName { get; private set; } = null!;

    /// <summary>
    /// Gets the vendor's display public name.
    /// </summary>
    public string DisplayName { get; private set; } = null!;

    /// <summary>
    /// Gets the vendor's professional summary bio.
    /// </summary>
    public string Bio { get; private set; } = null!;

    /// <summary>
    /// Gets the file path or URL for the vendor's logo image.
    /// </summary>
    public string? Logo { get; private set; }

    /// <summary>
    /// Gets the file path or URL for the vendor's cover background image.
    /// </summary>
    public string? CoverImage { get; private set; }

    /// <summary>
    /// Gets the average rating computed from customer reviews.
    /// </summary>
    public decimal AverageRating { get; private set; }

    /// <summary>
    /// Gets the total number of orders completed successfully by this vendor.
    /// </summary>
    public int CompletedOrders { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the vendor has been verified.
    /// </summary>
    public bool IsVerified { get; private set; }

    /// <summary>
    /// Gets the current status of the vendor profile.
    /// </summary>
    public VendorStatus Status { get; private set; } = VendorStatus.PendingApproval;

    // Navigation properties

    /// <summary>
    /// Gets the linked application user.
    /// </summary>
    public virtual ApplicationUser User { get; private set; } = null!;

    /// <summary>
    /// Gets the verification details for this vendor, if any.
    /// </summary>
    public virtual VendorVerification? Verification { get; private set; }

    /// <summary>
    /// Gets the read-only collection of weekly scheduling slot availabilities.
    /// </summary>
    public virtual IReadOnlyCollection<VendorAvailability> Availabilities => _availabilities.AsReadOnly();

    /// <summary>
    /// Gets the read-only collection of portfolio project showcases.
    /// </summary>
    public virtual IReadOnlyCollection<VendorPortfolio> PortfolioItems => _portfolioItems.AsReadOnly();

    // Parameterless constructor for EF Core materialization
    private VendorProfile()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorProfile"/> class.
    /// </summary>
    public VendorProfile(Guid userId, string businessName, string displayName, string bio, string? logo = null, string? coverImage = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        UserId = userId;
        UpdateProfileDetails(businessName, displayName, bio, logo, coverImage);
        AverageRating = 0.00m;
        CompletedOrders = 0;
        IsVerified = false;
        Status = VendorStatus.PendingApproval;
    }

    /// <summary>
    /// Updates the vendor's profile business parameters.
    /// </summary>
    public void UpdateProfileDetails(string businessName, string displayName, string bio, string? logo, string? coverImage)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name cannot be empty.", nameof(businessName));
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty.", nameof(displayName));
        if (string.IsNullOrWhiteSpace(bio))
            throw new ArgumentException("Bio cannot be empty.", nameof(bio));

        BusinessName = businessName.Trim();
        DisplayName = displayName.Trim();
        Bio = bio.Trim();
        Logo = logo?.Trim();
        CoverImage = coverImage?.Trim();
    }

    /// <summary>
    /// Verifies the vendor profile based on a validation submission.
    /// </summary>
    /// <param name="verification">The verification record.</param>
    public void Verify(VendorVerification verification)
    {
        ArgumentNullException.ThrowIfNull(verification);
        Verification = verification;
        IsVerified = true;
        Status = VendorStatus.Approved;
    }

    /// <summary>
    /// Suspends the vendor profile, preventing new listing creations or order intakes.
    /// </summary>
    public void Suspend()
    {
        Status = VendorStatus.Suspended;
    }

    /// <summary>
    /// Activates the vendor profile.
    /// </summary>
    public void Activate()
    {
        Status = IsVerified ? VendorStatus.Approved : VendorStatus.PendingApproval;
    }

    /// <summary>
    /// Increments the completed orders count upon successful fulfillment.
    /// </summary>
    public void IncrementCompletedOrders()
    {
        CompletedOrders++;
    }

    /// <summary>
    /// Updates the average review rating.
    /// </summary>
    /// <param name="rating">The computed average rating score.</param>
    public void UpdateAverageRating(decimal rating)
    {
        if (rating < 0.00m || rating > 5.00m)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0.00 and 5.00.");

        AverageRating = rating;
    }

    /// <summary>
    /// Adds a weekly availability schedule slot.
    /// </summary>
    public void AddAvailability(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, bool isAvailable = true)
    {
        if (startTime >= endTime)
            throw new ArgumentException("End time must be greater than start time.");

        // Check slot overlaps on the same day
        var hasOverlap = _availabilities.Any(a =>
            a.DayOfWeek == dayOfWeek &&
            a.IsAvailable &&
            isAvailable &&
            ((startTime >= a.StartTime && startTime < a.EndTime) ||
             (endTime > a.StartTime && endTime <= a.EndTime) ||
             (startTime <= a.StartTime && endTime >= a.EndTime)));

        if (hasOverlap)
            throw new InvalidOperationException($"Availability slot overlaps with an existing slot on {dayOfWeek}.");

        _availabilities.Add(new VendorAvailability(Id, dayOfWeek, startTime, endTime, isAvailable));
    }

    /// <summary>
    /// Adds a portfolio showcase item.
    /// </summary>
    public void AddPortfolioItem(string title, string description, string imageUrl, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Portfolio title is required.", nameof(title));

        // Check display order uniqueness
        if (_portfolioItems.Any(p => p.DisplayOrder == displayOrder))
            throw new InvalidOperationException($"A portfolio item with display order {displayOrder} already exists.");

        _portfolioItems.Add(new VendorPortfolio(Id, title, description, imageUrl, displayOrder));
    }
}
