using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using SportHub.Services;

namespace SportHub.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly IGetArticleService _service;
        public DetailsModel(IGetArticleService service)
        {
            _service = service;
        }

        public Article Article { get; set; }
        public string team;
        public string subcategory;
        public string category;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Article = await _service.GetArticle(id);
            try
            {
                team = _service.GetArticlesTeam(id);
                subcategory = _service.GetArticlesSubcategory(id);
                category = _service.GetArticlesCategory(id);
            }
            catch
            {
                return NotFound();
            }
            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
