using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.UserServiceExceptions
{
    public class EmailAlreadyInUseException : UserServiceException
    {
        public EmailAlreadyInUseException() : base($"User with such email already exists") { }
    }
}
