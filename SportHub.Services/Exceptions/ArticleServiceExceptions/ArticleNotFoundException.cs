using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Services.Exceptions.ArticleServiceExceptions
{
    public class ArticleNotFoundException: ArticleServiceException
    {
        public ArticleNotFoundException() : base($"Article with such id does not exist", 404) { }
    }
}