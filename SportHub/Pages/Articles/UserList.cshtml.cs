using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain.Models;
using SportHub.Services;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SportHub.Pages.Articles
{
    public class UserListModel : PageModel
    {
        private readonly IGetAdminArticlesService _articleService;
        private readonly IImageService _imageService;
        public UserListModel(IGetAdminArticlesService articleService, IImageService imageService)
        {
            _articleService = articleService;
            _imageService = imageService;
        }

        public string CategoryView { get; set; }
        public string SubCategoryView { get; set; }
        public string TeamView { get; set; }
        public IList<Article> Articles { get; set; }
        public int AmountOfArticles { get; set; }
        public int AmountBottomDisplayedArticles { get; set; }
        public Article HeaderArticle { get; set; }
        public async Task OnGetAsync(string category, string? subcategory, string? team)
        {
            CategoryView = category;
            SubCategoryView = subcategory;
            TeamView = team;
            Articles = await _articleService.GetUserArticles(category, subcategory, team);
            for(int i = 0; i < Articles.Count; i++)
            {
                Articles[i].ContentText = HttpUtility.HtmlDecode(Articles[i].ContentText);
                Articles[i].ContentText = Regex.Replace(Articles[i].ContentText, @"<[^>]+>", String.Empty);
            }
            AmountOfArticles = Articles.Count;
            if (Articles.Count != 0)
            {
                HeaderArticle = Articles[0];
            }
            if (AmountOfArticles < 10)
            {
                AmountBottomDisplayedArticles = AmountOfArticles;
            }
            else
            {
                AmountBottomDisplayedArticles = 10;
            }
        }
    }
}
