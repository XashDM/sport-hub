using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.ArticleServices;

namespace SportHub.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly GetArticleService _service;
        public DetailsModel(Domain.SportHubDBContext context, GetArticleService service)
        {
            _service = service;
        }


        public Article Article { get; set; }
        public string team;
        public string subcategory;
        public string category;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Article = _service.GetArticle(id);
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
