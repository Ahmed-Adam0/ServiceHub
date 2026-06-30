using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ServiceHub.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation property for session refresh tokens
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public ApplicationUser()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public ApplicationUser(string email, string fullName) : this()
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UserName = email;
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
    }
}

