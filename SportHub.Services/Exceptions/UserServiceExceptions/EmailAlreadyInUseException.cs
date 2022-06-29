using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.UserServiceExceptions
{
    public class EmailAlreadyInUseException : UserServiceException
    {
        public EmailAlreadyInUseException() : base($"Such user already exists", 409) { }
    }
}
