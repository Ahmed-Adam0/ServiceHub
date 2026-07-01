namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines categories for platform alert dispatches.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Alert about account status changes (e.g. suspension, approval).
    /// </summary>
    AccountStatusAlert = 1,

    /// <summary>
    /// Alert dispatched to vendor when a buyer purchases their service.
    /// </summary>
    NewOrderReceived = 2,

    /// <summary>
    /// Alert dispatched to buyer/vendor on order progression steps.
    /// </summary>
    OrderStatusUpdated = 3,

    /// <summary>
    /// Alert for new chat messages.
    /// </summary>
    NewChatMessage = 4,

    /// <summary>
    /// Alert for completed deposits or payout releases.
    /// </summary>
    PaymentProcessed = 5,

    /// <summary>
    /// Alert about feedback submissions.
    /// </summary>
    ReviewSubmitted = 6,

    /// <summary>
    /// Alert triggered by dispute cases updates.
    /// </summary>
    DisputeAlert = 7,

    /// <summary>
    /// Generic system-wide broadcast alerts.
    /// </summary>
    SystemAlert = 8
}
