using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Services.ArticleServices
{
    public class GetArticleService: IGetArticleService
    {
        private readonly SportHubDBContext _context;
        private readonly IImageService _imageService;
        NavigationItem NavigationItem;

        public GetArticleService(SportHubDBContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }


        public async Task<Article> GetArticle(int id)
        {
            try
            {
                var article = await _context.Articles.FirstOrDefaultAsync(idArticle => idArticle.Id == id);
                if (article is not null)
                {
                    article.ImageLink = await _imageService.GetImageLinkByNameAsync(article.ImageLink);
                }
                return article;
            }
            catch { return null; }
        }

        public string GetArticlesTeam(int? id)
        {
            var Article = _context.Articles.First(Article => Article.Id == id);
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
            var Article = _context.Articles.First(Article => Article.Id == id);
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
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.ParentsItemId);
                if (NavigationItem.Type != "Subcategory") return null;
            }
            return NavigationItem.Name;
        }

        public string GetArticlesCategory(int? id)
        {
            var Article = _context.Articles.First(Article => Article.Id == id);
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
                NavigationItem = _context.NavigationItems.First(Item => Item.Id == NavigationItem.ParentsItemId);
                count++;
            }
            if (NavigationItem.Type != "Category") return null;
            return NavigationItem.Name;
        }
    }
}
