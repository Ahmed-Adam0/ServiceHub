namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines moderation workflows for user abuse or fraud reports.
/// </summary>
public enum ReportStatus
{
    /// <summary>
    /// Report has been submitted and is in queue.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// Report is currently under investigation by moderation agents.
    /// </summary>
    UnderReview = 2,

    /// <summary>
    /// Investigation completed, no policy violations were found.
    /// </summary>
    ResolvedNoAction = 3,

    /// <summary>
    /// Violation confirmed, disciplinary actions applied to target listing/profile.
    /// </summary>
    ResolvedActionTaken = 4,

    /// <summary>
    /// Report was dismissed as invalid or spam.
    /// </summary>
    Dismissed = 5
}
