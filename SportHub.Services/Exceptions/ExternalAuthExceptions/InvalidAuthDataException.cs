using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.ExternalAuthExceptions
{
    public class InvalidAuthDataException : ExternalAuthException
    {
        public InvalidAuthDataException() : base($"Invalid or insufficient auth data provided", 400) { }
    }
}