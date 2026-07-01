using System;
using ServiceHub.Domain.Common.Base;

namespace ServiceHub.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = null!;
    public DateTime Expiry { get; set; }
    public bool IsRevoked { get; set; }
    public string? CreatedByIp { get; set; }

    // Navigation property
    public virtual ApplicationUser User { get; set; } = null!;

    public RefreshToken()
    {
    }

    public RefreshToken(Guid userId, string tokenHash, DateTime expiry, string? createdByIp)
    {
        UserId = userId;
        TokenHash = tokenHash ?? throw new ArgumentNullException(nameof(tokenHash));
        Expiry = expiry;
        CreatedByIp = createdByIp;
    }
}

