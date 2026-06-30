using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMarketplace.Domain.Entities;

namespace SmartMarketplace.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);
    }
}
Sprint 0 has been completed and approved.

Now start Sprint 1.

Module:
Identity & Authentication

The project foundation is already implemented.

Do NOT regenerate any existing code.

Reuse the current architecture and build on top of it.

====================================================

Implement the following features in the correct enterprise order.

1. User Registration

- Register as Customer only.
- Validate all inputs.
- Check duplicate Email.
- Check duplicate Username (if applicable).
- Hash password using ASP.NET Identity.
- Generate Email Confirmation Token.
- Return a clean response.

----------------------------------------------------

2. Email Confirmation

- Confirm email using token.
- Handle expired or invalid token.
- Prevent duplicate confirmation.

----------------------------------------------------

3. Login

- Validate credentials.
- Reject unconfirmed emails.
- Reject locked accounts.
- Generate JWT Access Token.
- Generate Refresh Token.
- Save Refresh Token securely.
- Return authentication response.

----------------------------------------------------

4. Refresh Token

- Validate Refresh Token.
- Rotate Refresh Token.
- Revoke old token.
- Generate new JWT.
- Generate new Refresh Token.

----------------------------------------------------

5. Logout

- Revoke current Refresh Token.
- Logout from current device.

----------------------------------------------------

6. Change Password

- Validate current password.
- Validate new password.
- Update password securely.

----------------------------------------------------

7. Forgot Password

- Generate Reset Password Token.
- Return secure response.

----------------------------------------------------

8. Reset Password

- Validate Reset Token.
- Reset password.
- Invalidate previous sessions.

====================================================

For every feature generate:

- Application Layer
    - DTOs
    - Commands / Queries (CQRS if applicable)
    - Validators
    - Interfaces
    - AutoMapper Profiles

- Infrastructure Layer
    - Service Implementations

- API Layer
    - Endpoints
    - Request Models
    - Response Models
    - Authorization

Follow the existing Onion Architecture.

Use:
- .NET 10
- ASP.NET Identity
- JWT
- FluentValidation
- AutoMapper
- SOLID
- Clean Code

Generate production-ready code.

Do not modify completed foundation classes unless absolutely necessary.

Implement one feature at a time, beginning with User Registration.
Wait for approval before moving to the next feature.