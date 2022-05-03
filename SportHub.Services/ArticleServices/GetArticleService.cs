using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Linq;

namespace SportHub.Services.ArticleServices
{
    public class GetArticleService: IGetArticleService
    {
        private readonly SportHubDBContext _context;
        public GetArticleService(SportHubDBContext context)
        {
            _context = context;
        }

        Article Article;
        public Article GetArticle(int? id)
        {
            try
            {
                Article = _context.Articles.First(idArticle => idArticle.Id == id);
            }
            catch { return null; }
            return Article;
        }
        NavigationItem NavigationItem;
        public string GetArticlesTeam(int? id)
        {
            Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return null;
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
            }
            catch
            {
                return null;
            }
            if (NavigationItem.Type != "Team") return null;
            return NavigationItem.Name;
        }

        public string GetArticlesSubcategory(int? id)
        {
            Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return null;
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
                if (NavigationItem.Type == "Subcategory") return NavigationItem.Name;
            }
            catch
            {
                return null;
            }
            if (NavigationItem.Type == "Category")
            {
                return null;
            }
            if (NavigationItem.Type == "Team")
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.FatherItemId);
                if (NavigationItem.Type != "Subcategory") return null;
            }
            return NavigationItem.Name;
        }

        public string GetArticlesCategory(int? id)
        {
            Article = _context.Articles.First(Article => Article.Id == id);
            if (Article.ReferenceItemId == null)
            {
                return "All Category";
            }
            try
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == Article.ReferenceItemId);
            }
            catch
            {
                return null;
            }
            int count = 0;
            while (NavigationItem.Type != "Category" && count < 3)
            {
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.FatherItemId);
                count++;
            }
            if (NavigationItem.Type != "Category") return null;
            return NavigationItem.Name;
        }
    }
}
