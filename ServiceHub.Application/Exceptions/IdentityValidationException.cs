using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ServiceHub.Application.Exceptions;

public class IdentityValidationException : IdentityBusinessException
{
    public IEnumerable<string> Errors { get; }

    public IdentityValidationException(IEnumerable<IdentityError> errors)
        : base($"Identity operation failed: {string.Join(", ", errors.Select(e => e.Description))}")
    {
        Errors = errors.Select(e => e.Description);
    }
}
