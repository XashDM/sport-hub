using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.ExternalAuthExceptions
{
    public class InvalidAuthProviderException : ExternalAuthException
    {
        public InvalidAuthProviderException() : base($"Invalid auth provider given", 400) { }
    }
}
