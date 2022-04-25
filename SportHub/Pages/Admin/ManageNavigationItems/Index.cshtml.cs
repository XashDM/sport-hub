using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;

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
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnGetRoot()
        {
            return new OkObjectResult(_servise.GetRoute());
        }
        public IActionResult OnGetChildren( int ItemId)
        {
            
            return new OkObjectResult(_servise.GetChildrenOfItem(ItemId));
        }
        public IActionResult OnGetAddItem(string name, int? fatherItemId, string Type)
        {
            NavigationItem Item = new NavigationItem {
                Type = Type,
                Name = name,
                FatherItemId = fatherItemId,
            };
            _servise.AddNewItem(Item);
            return new OkObjectResult(Item);
        }
    }
}
