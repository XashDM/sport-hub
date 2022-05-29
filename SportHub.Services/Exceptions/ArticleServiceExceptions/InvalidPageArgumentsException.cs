using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.ArticleServiceExceptions
{
    public class InvalidPageArgumentsException : ArticleServiceException
    {
        public InvalidPageArgumentsException() : base($"Invalid page arguments provided", 400) { }
    }
}