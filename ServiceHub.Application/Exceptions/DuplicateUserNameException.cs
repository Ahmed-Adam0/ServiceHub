namespace ServiceHub.Application.Exceptions;

public class DuplicateUserNameException : IdentityBusinessException
{
    public DuplicateUserNameException(string userName) 
        : base($"Username '{userName}' is already taken.") { }
}
