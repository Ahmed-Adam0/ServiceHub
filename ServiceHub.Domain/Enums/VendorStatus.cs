namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines verification and review stages for vendor profiles.
/// </summary>
public enum VendorStatus
{
    /// <summary>
    /// Profile is pending administrative review.
    /// </summary>
    PendingApproval = 1,

    /// <summary>
    /// Profile has been approved to publish services.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Profile is temporarily or permanently suspended.
    /// </summary>
    Suspended = 3,

    /// <summary>
    /// Profile approval has been rejected.
    /// </summary>
    Rejected = 4
}
