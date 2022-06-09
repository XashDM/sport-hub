using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain;
using SportHub.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using SportHub.Views;
using SportHub.Services;

namespace SportHub.Controllers
{
    public class AdminViewAndManageArticlesController : Controller
    {
        SportHubDBContext _context;
        IGetAdminArticlesService _allArticlesService;
        IImageService _imageService;
        IGetArticleService _articleService;

        public AdminViewAndManageArticlesController(SportHubDBContext context, IGetAdminArticlesService allArticlesService, IImageService imageService, IGetArticleService articleService)
        {
            _context = context;
            _allArticlesService = allArticlesService;
            _imageService = imageService;
            _articleService = articleService;   
        }

        [HttpPost]
        [Route("/articles")]
        public async Task<IActionResult> Articles([FromBody] ArticlesDisplayVariables articleInfo)
        {
            IList<Article> articles = _allArticlesService.GetArticlesRange(articleInfo.startPosition, articleInfo.amountArticles, articleInfo.publishValue, articleInfo.category, articleInfo.subcategory, articleInfo.team);
            for (int i = 0; i < articles.Count; i++)
            {
                articles[i].ImageLink = await _imageService.GetImageLinkByNameAsync(articles[i].ImageLink);
            }
            return new OkObjectResult(articles);
        }

        [HttpDelete]
        [Route("/article/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = _context.Articles.FirstOrDefault(article => article.Id.Equals(id));
            if (article != null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
                return new OkObjectResult(article);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("/article/publishunpublish/{id}")]
        public async Task<IActionResult> PublishUnpublish(int id)
        {
            var article = _context.Articles.FirstOrDefault(article => article.Id.Equals(id));
            if (article != null)
            {
                article.IsPublished = !article.IsPublished;
                _context.SaveChanges();
                return new OkObjectResult(article);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("/article/move/{id}/{categoryId}")]
        public async Task<IActionResult> PublishUnpublish(int id, int categoryId)
        {
            string resultChange = _articleService.ChangeArticlesCategory(id, categoryId);
            return new OkObjectResult(resultChange);
        }
    }
}