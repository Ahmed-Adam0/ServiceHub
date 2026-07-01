namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines states of external billing operations.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment has been initiated but is pending validation.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Payment processing is in execution at the gateway.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Payment was successfully captured.
    /// </summary>
    Succeeded = 3,

    /// <summary>
    /// Payment failed at validation or gateway level.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Payment requires additional actions (e.g. 3D-Secure challenges).
    /// </summary>
    RequiresAction = 5,

    /// <summary>
    /// Payment was fully returned to the original account.
    /// </summary>
    Refunded = 6,

    /// <summary>
    /// Payment was partially returned to the original account.
    /// </summary>
    PartiallyRefunded = 7
}
