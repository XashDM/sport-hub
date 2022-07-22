using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenIsUsedException : TokenServiceException
    {
        public TokenIsUsedException() : base($"Provided token is already used", 400) { }
    }
}
