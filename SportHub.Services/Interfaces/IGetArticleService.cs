using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IGetArticleService
    {
        Task<Article> GetArticle(int id);
        string GetArticlesTeam(int? id);
        string GetArticlesSubcategory(int? id);
        string GetArticlesCategory(int? id);
        IQueryable<NavigationItem> GetAllCategoriesQueryable();
        IQueryable<NavigationItem> GetAllSubcategoriesByCategoryIdQueryable(int categoryId);
        IQueryable<NavigationItem> GetAllTeamsBySubcategoryIdQueryable(int subcategoryId);
        IQueryable<Article> GetAllArticlesByTeamIdQueryable(int teamId);
        (IQueryable<T>, int, int) Paginate<T>(IQueryable<T> items, int pageSize, int pageNumber);
        Task ApplyMainArticlesDisplayChanges(Dictionary<int, bool> articlesToSave);
        Task<DisplayItem[]> GetMainArticles();
    }
}
