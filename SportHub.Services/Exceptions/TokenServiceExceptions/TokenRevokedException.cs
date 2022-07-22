using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenRevokedException : TokenServiceException
    {
        public TokenRevokedException() : base($"Provided token is revoked", 400) { }
    }
}
