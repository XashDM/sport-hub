using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using System.Linq;

namespace SportHub.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly IGetArticleService _articleService;

        public AdminPageModel(IGetArticleService articleService)
        {
            _articleService = articleService;
        }
        public void OnGet()
        {
            
        }
    }
}
