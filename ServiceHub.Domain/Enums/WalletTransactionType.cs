namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines categories for wallet transactions and balance shifts.
/// </summary>
public enum WalletTransactionType
{
    /// <summary>
    /// Credit deposit made from external payment methods.
    /// </summary>
    Deposit = 1,

    /// <summary>
    /// Debit withdrawal out to bank or external wallets.
    /// </summary>
    Withdrawal = 2,

    /// <summary>
    /// Balance locked in escrow for order fulfillment starting.
    /// </summary>
    EscrowLock = 3,

    /// <summary>
    /// Balance released from escrow to vendor wallet.
    /// </summary>
    EscrowRelease = 4,

    /// <summary>
    /// Escrow balance returned to buyer wallet.
    /// </summary>
    EscrowRefund = 5,

    /// <summary>
    /// Platform fee commission deducted from order payouts.
    /// </summary>
    PlatformCommissionFee = 6,

    /// <summary>
    /// Balance adjustment made by administrators or disputes handlers.
    /// </summary>
    Adjustment = 7
}
