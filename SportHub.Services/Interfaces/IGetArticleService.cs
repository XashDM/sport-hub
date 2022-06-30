using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IGetArticleService
    {
        Task<Article> GetArticle(int id);
        string ChangeArticlesCategory(int id, int categoryId);
        string GetArticlesTeam(int? id);
        string GetArticlesSubcategory(int? id);
        string GetArticlesCategory(int? id);
        IQueryable<NavigationItem> GetAllCategoriesQueryable();
        IQueryable<NavigationItem> GetAllSubcategoriesByCategoryIdQueryable(int categoryId);
        IQueryable<NavigationItem> GetAllTeamsByParentIdQueryable(int subcategoryId);
        IQueryable<Article> GetAllArticlesByParentIdQueryable(int teamId);
        (IQueryable<T>, int, int) Paginate<T>(IQueryable<T> items, int pageSize, int pageNumber);
        Task SaveMainArticles(Dictionary<int, bool> articlesToSave);
        Task<DisplayItem[]> GetMainArticles();
        Task<DisplayItem[]> GetDisplayedMainArticles();
        Task<bool> UploadPhotoOfTheDay(ImageItem image);
        Task<DisplayItem> GetPhotoOfTheDay();
        Task<DisplayItem> GetDisplayedPhotoOfTheDay();
        Task DisplayPhotoOfTheDay();
        Task HidePhotoOfTheDay();
        Task<NavigationItem[]> GetAllCategoriesArrayAsync();
        Task<NavigationItem[]> GetAllSubcategoriesByCategoryIdArrayAsync(int categoryId);
        Task<NavigationItem[]> GetAllTeamsByParentIdArrayAsync(int parentId);
        Task<Article[]> GetArticlesByParentIdPaginatedArrayAsync(int parentId, int pageSize, int pageNumber);
        MainComment CreateMainComment(string message, int articleId, int userId);
    }
}
