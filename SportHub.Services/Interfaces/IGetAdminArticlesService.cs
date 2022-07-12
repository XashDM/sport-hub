using SportHub.Domain.Models;
using System.Collections.Generic;

namespace SportHub.Services.Interfaces
{
    public interface IGetAdminArticlesService
    {
        IList<Article> GetArticles(string? category, string? subcategory, string? team);
        IList<Article> GetArticlesByPublished(string? publishValue, string? category, string? subcategory, string? team);
        IList<Article> GetArticlesRange(int start, int end, string? publishValue, string? category, string? subcategory, string? team);
        IList<NavigationItem> GetCategories();
        IList<NavigationItem> GetCategoriesToMove(string? category);
        IList<NavigationItem> GetSubcategories(string category);
        IList<NavigationItem> GetTeams(string subcategory);
    }
}
