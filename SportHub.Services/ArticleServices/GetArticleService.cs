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
            var Article = _context.Articles.FirstOrDefault(Article => Article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItem is not null)
                {
                    NavigationItem = _context.NavigationItems.FirstOrDefault(Item => Item.Id == Article.ReferenceItemId);
                    if (NavigationItem is not null)
                    {
                        if (NavigationItem.Type != "Team")
                        {
                            return "";
                        }
                        return NavigationItem.Name;
                    }
                    //return error_page, idk how, but return
                    return "error_page";
                }
                return "";
            }
            return "error_page_404";
        }

        public string GetArticlesSubcategory(int? id)
        {
            var Article = _context.Articles.FirstOrDefault(article => article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItemId is not null)
                {
                    NavigationItem itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == Article.ReferenceItemId);
                    int count = 0;
                    while (itemReference.Type != "Subcategory" && count < 3)
                    {
                        itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == itemReference.ParentsItemId);
                        if (itemReference is null)
                        {
                            return "item reference wasn't found(no subcategory)";
                        }
                        if (itemReference.Type == "Category")
                        {
                            return "All subcategory";
                        }
                        count++;
                    }
                    if (itemReference.Type == "Subcategory")
                    {
                        return itemReference.Name;
                    }
                    return "subcategory wasn't found";
                }
                return "articles reference id is null";
            }
            return "error_page_404";
        }

        public string GetArticlesCategory(int? id)
        {
            var Article = _context.Articles.FirstOrDefault(article => article.Id == id);
            if (Article is not null)
            {
                if (Article.ReferenceItemId is not null)
                {
                    NavigationItem itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == Article.ReferenceItemId);
                    int count = 0;
                    while (itemReference.Type != "Category" && count < 3)
                    {
                        itemReference = _context.NavigationItems.FirstOrDefault(item => item.Id == itemReference.ParentsItemId);
                        if (itemReference is null)
                        {
                            return "item reference wasn't found";
                        }
                        count++;
                    }
                    if (itemReference.Type == "Category")
                    {
                        return itemReference.Name;
                    }
                    return "category wasn't found";
                }
                return "articles reference id is null";
            }
            return "error_page_404";
        }
    }
}
