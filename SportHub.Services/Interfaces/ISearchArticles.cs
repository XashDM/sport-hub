using System.Collections.Generic;
using System.Linq;
using SportHub.Domain.Models;

namespace SportHub.Services.Interfaces
{
    public interface ISearchArticles
    {
        IList<Article> ArticlesBySearch(string search);
        IList<Article> ArticlesBySearchRange(string search, int startPosition, int endPosition);
        IQueryable<Article> SearchArticlesSecond(string search);
    }
}
