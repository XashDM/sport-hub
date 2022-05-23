using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportHub.Domain;
using SportHub.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    public class ArticleController : Controller
    {
        SportHubDBContext _context;
        public ArticleController(SportHubDBContext context)
        {
            _context = context;
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