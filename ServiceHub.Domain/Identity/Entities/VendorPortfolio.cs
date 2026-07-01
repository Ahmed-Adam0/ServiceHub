using System;
using ServiceHub.Domain.Common.Base;

namespace ServiceHub.Domain.Identity.Entities;

/// <summary>
/// Represents previous projects and showcases completed by the vendor.
/// </summary>
public class VendorPortfolio : SoftDeleteEntity
{
    /// <summary>
    /// Gets the unique identifier of the linked vendor profile.
    /// </summary>
    public Guid VendorProfileId { get; private set; }

    /// <summary>
    /// Gets the project title.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Gets the project summary details description.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets the URL of the showcase image or attachment.
    /// </summary>
    public string ImageUrl { get; private set; } = null!;

    /// <summary>
    /// Gets the sorting order on public catalogs.
    /// </summary>
    public int DisplayOrder { get; private set; }

    // Navigation property

    /// <summary>
    /// Gets the linked vendor profile.
    /// </summary>
    public virtual VendorProfile VendorProfile { get; private set; } = null!;

    // Parameterless constructor for EF Core materialization
    private VendorPortfolio()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorPortfolio"/> class.
    /// </summary>
    public VendorPortfolio(Guid vendorProfileId, string title, string description, string imageUrl, int displayOrder)
    {
        if (vendorProfileId == Guid.Empty)
            throw new ArgumentException("Vendor profile ID cannot be empty.", nameof(vendorProfileId));

        VendorProfileId = vendorProfileId;
        UpdateDetails(title, description, imageUrl, displayOrder);
    }

    /// <summary>
    /// Updates the portfolio project details.
    /// </summary>
    public void UpdateDetails(string title, string description, string imageUrl, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Project title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Project description is required.", nameof(description));
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Showcase image URL is required.", nameof(imageUrl));
        if (displayOrder < 0)
            throw new ArgumentException("Display order must be greater than or equal to 0.", nameof(displayOrder));

        Title = title.Trim();
        Description = description.Trim();
        ImageUrl = imageUrl.Trim();
        DisplayOrder = displayOrder;
    }
}
