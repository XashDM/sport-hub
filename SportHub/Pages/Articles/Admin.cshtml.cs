#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;

namespace SportHub.Pages.Articles
{
    public class IndexModel : PageModel
    {
        private readonly IGetAdminArticlesService _service;
        private readonly SportHubDBContext _context;
        public IndexModel(IGetAdminArticlesService service, SportHubDBContext context)
        {
            _service = service;
            _context = context;
        }

        public IList<Article> Article { get;set; }
        public IList<NavigationItem> SubCategories { get; set; }
        public IList<NavigationItem> Teams { get;set; }
        public SelectList SelectSubcategory { get; set; }
        public SelectList SelectTeam { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedSubcategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedTeam { get; set; }
        public async Task OnGetAsync(string? category)
        {
            Article = _service.GetArticles(category, SelectedSubcategory, SelectedTeam);
            if (category != null)
            {
                SubCategories = _service.GetSubcategories(category);
                
                if (SubCategories != null)
                {
                    SelectSubcategory = new SelectList(SubCategories, nameof(NavigationItem.Name), nameof(NavigationItem.Name));
                    SelectedTeam = "SelectedTeam";
                }
            }
            if (SelectedSubcategory != null && category != null)
            {
                Teams = _service.GetTeams(SelectedSubcategory);
                if (Teams != null)
                {
                    SelectTeam = new SelectList(Teams, nameof(NavigationItem.Name), nameof(NavigationItem.Name));
                }
            }
        }
        public async Task OnPostAsync(string category)
        {
            Article = _service.GetArticles(category, SelectedSubcategory, SelectedTeam);
        }
    }
}
