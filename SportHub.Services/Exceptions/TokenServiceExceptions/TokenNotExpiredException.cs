using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenNotExpiredException : TokenServiceException
    {
        public TokenNotExpiredException() : base($"Token has not yet expired", 400) { }
    }
}
