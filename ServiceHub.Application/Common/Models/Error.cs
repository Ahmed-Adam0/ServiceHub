using System.Collections.Generic;

namespace ServiceHub.Application.Common.Models;

public record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", ErrorType.Failure);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5
}

public record ValidationError : Error
{
    public IEnumerable<string> Errors { get; }

    public ValidationError(IEnumerable<string> errors)
        : base("Validation.Error", "One or more validation errors occurred.", ErrorType.Validation)
    {
        Errors = errors;
    }
}
