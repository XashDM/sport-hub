using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IGetArticleService _articleService;
        public DisplayItem PhotoOfTheDay;

        public IndexModel(ILogger<IndexModel> logger, IGetArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        public async Task<IActionResult> OnGet()
        {
            PhotoOfTheDay = await _articleService.GetDisplayedPhotoOfTheDay();
            return Page();
        }
    }
}