using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IGetAdminArticlesService
    {
        IList<Article> GetArticles(string? category, string? subcategory, string? team);
        Task<IList<Article>> GetUserArticles(string? category, string? subcategory, string? team);
        IList<Article> GetArticlesBySearch(string? publishValue, string? category, string? subcategory, string? team, string? search);
        IList<Article> GetArticlesByPublished(string? publishValue, string? category, string? subcategory, string? team);
        IList<Article> GetArticlesRange(int start, int end, string? publishValue, string? category, string? subcategory, string? team, string? search);
        IList<NavigationItem> GetCategories();
        IList<NavigationItem> GetCategoriesToMove(string? category);
        IList<NavigationItem> GetSubcategories(string category);
        IList<NavigationItem> GetTeams(string subcategory);
    }
}
