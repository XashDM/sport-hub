using System.Collections.Generic;
using System.Linq;
using SportHub.Domain.Models;

namespace SportHub.Services.Interfaces
{
    public interface ISearchService
    {
        IList<Article> ArticleSearch(string search);
        IList<Article> ArticleSearchLimits(string search, int startPosition, int endPosition);
        IQueryable<Article> ArticleSearchAllTree(string search);
        int ArticleSearchAmount(string search);
    }
}
