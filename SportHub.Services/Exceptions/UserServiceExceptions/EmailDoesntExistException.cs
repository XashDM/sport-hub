using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.UserServiceExceptions
{
    public class EmailDoesntExistException : UserServiceException
    {
        public EmailDoesntExistException() : base($"User with such email doesn't exists") { }
    }
}
