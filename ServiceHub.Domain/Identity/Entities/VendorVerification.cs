using System;
using ServiceHub.Domain.Common.Base;
using ServiceHub.Domain.Enums;

namespace ServiceHub.Domain.Identity.Entities;

/// <summary>
/// Stores verification details and approval metadata submitted by vendors.
/// </summary>
public class VendorVerification : AuditableEntity
{
    /// <summary>
    /// Gets the unique identifier of the linked vendor profile.
    /// </summary>
    public Guid VendorProfileId { get; private set; }

    /// <summary>
    /// Gets the vendor's national ID or passport serial number.
    /// </summary>
    public string NationalId { get; private set; } = null!;

    /// <summary>
    /// Gets the storage file path or URL for the verification document.
    /// </summary>
    public string DocumentPath { get; private set; } = null!;

    /// <summary>
    /// Gets the verification review status.
    /// </summary>
    public VendorStatus VerificationStatus { get; private set; } = VendorStatus.PendingApproval;

    /// <summary>
    /// Gets the timestamp when the verification was approved.
    /// </summary>
    public DateTime? VerifiedAt { get; private set; }

    /// <summary>
    /// Gets the explanation notes or rejection reason details, if any.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation property

    /// <summary>
    /// Gets the linked vendor profile.
    /// </summary>
    public virtual VendorProfile VendorProfile { get; private set; } = null!;

    // Parameterless constructor for EF Core materialization
    private VendorVerification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorVerification"/> class.
    /// </summary>
    public VendorVerification(Guid vendorProfileId, string nationalId, string documentPath)
    {
        if (vendorProfileId == Guid.Empty)
            throw new ArgumentException("Vendor profile ID cannot be empty.", nameof(vendorProfileId));
        if (string.IsNullOrWhiteSpace(nationalId))
            throw new ArgumentException("National ID is required.", nameof(nationalId));
        if (string.IsNullOrWhiteSpace(documentPath))
            throw new ArgumentException("Document file path is required.", nameof(documentPath));

        VendorProfileId = vendorProfileId;
        NationalId = nationalId.Trim();
        DocumentPath = documentPath.Trim();
        VerificationStatus = VendorStatus.PendingApproval;
    }

    /// <summary>
    /// Approves the verification document.
    /// </summary>
    public void Approve()
    {
        VerificationStatus = VendorStatus.Approved;
        VerifiedAt = DateTime.UtcNow;
        Notes = "Approved successfully.";
    }

    /// <summary>
    /// Rejects the verification document with explanation details.
    /// </summary>
    /// <param name="reason">The rejection explanation reason.</param>
    public void Reject(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection explanation reason is required.", nameof(reason));

        VerificationStatus = VendorStatus.Rejected;
        VerifiedAt = null;
        Notes = reason.Trim();
    }
}
