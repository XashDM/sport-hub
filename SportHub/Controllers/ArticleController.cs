using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain;
using SportHub.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using SportHub.Domain.Views;
using SportHub.Services;

namespace SportHub.Controllers
{
    public class ArticleController : Controller
    {
        SportHubDBContext _context;
        IGetAdminArticlesService _articleService;
        IImageService _imageService;

        public ArticleController(SportHubDBContext context, IGetAdminArticlesService articleService, IImageService imageService)
        {
            _context = context;
            _articleService = articleService;
            _imageService = imageService;
        }

        [HttpPost]
        [Route("/articles")]
        public async Task<IActionResult> Articles([FromBody] ArticlesDisplayVariables articleInfo)
        {
            IList<Article> articles = _articleService.GetArticlesRange(articleInfo.startPosition, articleInfo.amountArticles, articleInfo.publishValue, articleInfo.category, articleInfo.subcategory, articleInfo.team);
            for (int i = 0; i < articles.Count; i++)
            {
                articles[i].ImageLink = await _imageService.GetImageLinkByNameAsync(articles[i].ImageLink);
            }
            return new OkObjectResult(articles);
        }

        // Delete: /article/delete/{id}
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
    }
}