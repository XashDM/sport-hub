using SportHub.Domain.Models;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface IGetArticleService
    {
        Task<Article> GetArticle(int? id);
        string GetArticlesTeam(int? id);
        string GetArticlesSubcategory(int? id);
        string GetArticlesCategory(int? id);
    }
}
