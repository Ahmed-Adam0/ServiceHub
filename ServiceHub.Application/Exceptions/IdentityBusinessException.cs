using System;

namespace ServiceHub.Application.Exceptions;

public class IdentityBusinessException : Exception
{
    public IdentityBusinessException(string message) : base(message) { }
}
