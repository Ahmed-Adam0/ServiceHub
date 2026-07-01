namespace ServiceHub.Application.Exceptions;

public class DuplicateEmailException : IdentityBusinessException
{
    public DuplicateEmailException(string email) 
        : base($"Email '{email}' is already registered.") { }
}
