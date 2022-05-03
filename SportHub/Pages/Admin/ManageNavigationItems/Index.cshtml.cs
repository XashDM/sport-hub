using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.Pages.Admin.ManageNavigationItems
{
    public class IndexModel : PageModel
    {
        private readonly Domain.SportHubDBContext _context;
        private INavigationItemService _servise;
        public IndexModel(Domain.SportHubDBContext context, INavigationItemService servise)
        {
            _context = context;
            _servise = servise;
        }
        public async Task<IActionResult> OnGetArticle(int ItemId)
        {
            return new OkObjectResult(await _servise.GetArticlesofItem(ItemId));
        }
        public async Task<IActionResult> OnGetTree(int ItemId)
        {
            return new OkObjectResult(await _servise.GetRecusiveTree(ItemId));
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnGetRoot()
        {
            return new OkObjectResult(await _servise.GetTopCategories());
        }
        public async Task<IActionResult> OnGetChildren( int ItemId)
        {
            
            return new OkObjectResult(await _servise.GetChildrenOfItem(ItemId));
        }
        public async Task<IActionResult> OnGetAddItem(string name, int? ParentsItemId, string Type)
        {
            NavigationItem Item = new NavigationItem {
                Type = Type,
                Name = name,
                ParentsItemId = ParentsItemId,
            };
            await _servise.AddNewItem(Item);
            return new OkObjectResult(Item);
        }
    }
}
