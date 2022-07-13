using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class AddArticleModel : PageModel
    {
        public int itemId { get; set; }
        public NavigationItem category { get; set; }

        private readonly ILogger<AddArticleModel> _logger;
        private readonly INavigationItemService _navigationService;

        public AddArticleModel(ILogger<AddArticleModel> logger, INavigationItemService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }
        public async Task<IActionResult> OnGetAsync(string? category)
        {
            if (category == null || category == "")
            {
                return NotFound();
            }
            this.category = await _navigationService.GetItemByName(category);
            if (this.category.ParentsItemId != null)
            {
                return NotFound();
            }
            this.itemId = this.category.Id;
            return Page();
        }
    }
}