using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.TokenServiceExceptions
{
    public class TokenClaimNotFoundOrInvalidException : TokenServiceException
    {
        public TokenClaimNotFoundOrInvalidException() : base($"Token claim is not found or invalid", 400, true) { }
    }
}
