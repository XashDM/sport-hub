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
        public int ItemId { get; set; }
        public NavigationItem Category { get; set; }

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
            this.Category = await _navigationService.GetItemByName(category);
            if (this.Category == null)
            {
                return NotFound();
            }
            if (this.Category.ParentsItemId != null)
            {
                return NotFound();
            }
            this.ItemId = this.Category.Id;
            return Page();
        }
    }
}