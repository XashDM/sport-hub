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
using SportHub.Services;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;

namespace SportHub.Pages.Articles
{
    public class IndexModel : PageModel
    {
        private readonly IGetAdminArticlesService _service;
        private readonly IImageService _imageService;
        public IndexModel(IGetAdminArticlesService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;

        }

        public IList<Article> Article { get;set; }
        public IList<NavigationItem> Categories { get; set; }
        public IList<NavigationItem> SubCategories { get; set; }
        public IList<NavigationItem> Teams { get;set; }
        public SelectList SelectSubcategory { get; set; }
        public SelectList SelectTeam { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedSubcategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedTeam { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PublishField { get; set; }
        public async Task OnGetAsync(string? category)
        {
            Categories = _service.GetCategories();
            Article = _service.GetArticlesRange(0,10,PublishField, category, SelectedSubcategory, SelectedTeam);
            for(int i = 0; i < Article.Count; i++)
            {
                Article[i].ImageLink = await _imageService.GetImageLinkByNameAsync(Article[i].ImageLink);
            }
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
            //Article = _service.GetArticles(category, SelectedSubcategory, SelectedTeam, PublishField);
        }
    }
}
