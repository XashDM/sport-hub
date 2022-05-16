using SportHub.Domain.Models;
using System.Collections.Generic;

namespace SportHub.Services.Interfaces
{
    public interface IGetAdminArticlesService
    {
        IList<Article> GetArticles(string? category, string? subcategory, string? team);
        IList<NavigationItem> GetSubcategories(string category);
        IList<NavigationItem> GetTeams(string subcategory);
    }
}
