namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines states of execution lifecycles for service orders.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order has been created but not initialized.
    /// </summary>
    Created = 1,

    /// <summary>
    /// Order is awaiting buyer payment validation.
    /// </summary>
    PendingPayment = 2,

    /// <summary>
    /// Payment is locked in escrow, work is currently in progress.
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// Vendor submitted delivery files, awaiting buyer review.
    /// </summary>
    Delivered = 4,

    /// <summary>
    /// Delivery has been approved and funds released to the vendor.
    /// </summary>
    Completed = 5,

    /// <summary>
    /// Order is subject to dispute resolution investigations.
    /// </summary>
    Disputed = 6,

    /// <summary>
    /// Order cancelled by mutual agreement or administrative actions.
    /// </summary>
    Cancelled = 7,

    /// <summary>
    /// Order cancelled and funds refunded back to the buyer.
    /// </summary>
    Refunded = 8
}
