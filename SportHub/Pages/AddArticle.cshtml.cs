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
        private readonly ILogger<AddArticleModel> _logger;
        private readonly INavigationItemService _navigationService;
        public int itemId { get; set; }
        public NavigationItem Category { get; set; }
        public AddArticleModel(ILogger<AddArticleModel> logger, INavigationItemService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }
        public async Task OnGetAsync(string? category)
        {
            if (category == null || category == "")
            {
                this.itemId = 1;
            }
            this.Category = await _navigationService.GetItemByName(category);
            this.itemId = this.Category.Id;
        }
    }
}