namespace ServiceHub.Domain.Enums;

/// <summary>
/// Defines user profile classifications within the ServiceHub platform.
/// </summary>
public enum UserType
{
    /// <summary>
    /// Represents standard buyers purchasing services.
    /// </summary>
    Customer = 1,

    /// <summary>
    /// Represents sellers offering listing packages.
    /// </summary>
    Vendor = 2,

    /// <summary>
    /// Represents platform admins and moderation handlers.
    /// </summary>
    Administrator = 3
}
