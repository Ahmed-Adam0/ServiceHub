namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines publishing status states for vendor service listings.
/// </summary>
public enum ServiceStatus
{
    /// <summary>
    /// Service is a draft and not visible in the marketplace.
    /// </summary>
    Draft = 1,

    /// <summary>
    /// Service has been submitted for moderation review.
    /// </summary>
    PendingApproval = 2,

    /// <summary>
    /// Service is active and available for customer orders.
    /// </summary>
    Active = 3,

    /// <summary>
    /// Service has been paused by the vendor.
    /// </summary>
    Paused = 4,

    /// <summary>
    /// Service was rejected during administrative moderation reviews.
    /// </summary>
    Rejected = 5,

    /// <summary>
    /// Service is suspended due to violations or vendor inactivity.
    /// </summary>
    Suspended = 6
}
