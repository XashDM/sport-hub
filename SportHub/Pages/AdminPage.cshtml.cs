using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SportHub.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly IGetAdminArticlesService _articleService;

        public AdminPageModel(IGetAdminArticlesService articleService)
        {
            _articleService = articleService;
        }

        public IList<NavigationItem> Categories { get; set; }

        public void OnGet()
        {
            Categories = _articleService.GetCategories();
        }
    }
}
