using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Article> articles = new List<Article> ();
        public List<string> teams = new List<string> ();
        IGetArticleService _getArticleService;
        public int Counter = 4;
        public IndexModel(ILogger<IndexModel> logger, IGetArticleService getArticleService)
        {
            _logger = logger;
            _getArticleService = getArticleService;
        }

        public async Task<IActionResult> OnGet()
        {
            Article article;
            string team = "";
            for (int i = 4; i <= Counter+3; i++)
            {
                article = await _getArticleService.GetArticle(i);
                team = _getArticleService.GetArticlesTeam(i);
                articles.Add(article);
                teams.Add(team);
            }
            return Page();
        }

        public async Task<IActionResult> OnGetTest1()
        {
            Article article;
            for (int i = 4; i <= Counter + 3; i++)
            {
                article = await _getArticleService.GetArticle(i);
                articles.Add(article);
            }
            return new OkObjectResult(articles);
        }
    }
}