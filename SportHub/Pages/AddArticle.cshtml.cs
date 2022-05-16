using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SportHub.Pages
{
    public class AddArticleModel : PageModel
    {
        private readonly ILogger<AddArticleModel> _logger;

        public AddArticleModel(ILogger<AddArticleModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}