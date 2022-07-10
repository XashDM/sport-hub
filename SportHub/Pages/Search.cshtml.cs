using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Services;
using SportHub.Services.Interfaces;
using SportHub.Views;
using System.Collections.Generic;
using SportHub.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace SportHub.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IGetArticleService _articleService;
        private readonly ISearchArticles _searchArticles;

        public SearchModel(IGetArticleService articleService, ISearchArticles searchArticles)
        {
            _articleService = articleService;
            _searchArticles = searchArticles;
        }

        public IList<ArticleForSearchResult?> ArticlesSearch { get; set; }
        public IList<Article> Articles { get; set; }
        public string title { get; set; }
        public int amountOfFindedTitles { get; set; }
        public bool IsOdmen { get; set; }
        
        [Authorize(Roles = "Admin")]
        public void OnGetAuthorized(string? searchValue)
        {
            IsOdmen = true;
        }

        public void OnGet(string? searchValue)
        {
            title = searchValue;
            Articles = _searchArticles.ArticlesBySearchRange(searchValue, 0, 10);
            ArticlesSearch = new List<ArticleForSearchResult>();
            for (int i = 0; i < Articles.Count; i++)
            {
                ArticleForSearchResult articleForSearchResult = new ArticleForSearchResult();
                articleForSearchResult.Id = Articles[i].Id;
                articleForSearchResult.ContentText = Articles[i].ContentText;
                articleForSearchResult.Category = _articleService.GetArticlesCategory(Articles[i].Id);
                articleForSearchResult.Subcategory = _articleService.GetArticlesSubcategory(Articles[i].Id);
                articleForSearchResult.Team = _articleService.GetArticlesTeam(Articles[i].Id);
                ArticlesSearch.Add(articleForSearchResult);
            }
            amountOfFindedTitles = _searchArticles.ArticlesBySearch(searchValue).Count;
        }        
    }
}
