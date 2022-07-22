using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenExpiredException : TokenServiceException
    {
        public TokenExpiredException() : base($"Token has expired", 400) { }
    }
}
