using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class InvalidTokenException : TokenServiceException
    {
        public InvalidTokenException() : base($"Provided token is invalid", 403) { }
    }
}
