using SportHub.Domain.Models;

namespace SportHub.Services.Interfaces
{
    public interface IGetArticleService
    {
        Article GetArticle(int? id);
        string GetArticlesTeam(int? id);
        string GetArticlesSubcategory(int? id);
        string GetArticlesCategory(int? id);
    }
}
