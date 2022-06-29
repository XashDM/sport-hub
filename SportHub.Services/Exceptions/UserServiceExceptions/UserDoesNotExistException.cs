using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.UserServiceExceptions
{
    public class UserDoesNotExistException : UserServiceException
    {
        public UserDoesNotExistException() : base($"Such user does not exist", 404) { }
    }
}
