namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines publishing status states for client feedback reviews.
/// </summary>
public enum ReviewStatus
{
    /// <summary>
    /// Review is public and active in the catalog listings.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Review has been flagged by users and is pending check.
    /// </summary>
    Flagged = 2,

    /// <summary>
    /// Review has been hidden by moderators for violations.
    /// </summary>
    Hidden = 3,

    /// <summary>
    /// Review is archived.
    /// </summary>
    Archived = 4
}
