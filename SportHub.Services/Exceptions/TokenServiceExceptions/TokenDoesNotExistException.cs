using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenDoesNotExistException : TokenServiceException
    {
        public TokenDoesNotExistException() : base($"Provided token does non exist", 404) { }
    }
}
