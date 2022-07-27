using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Services;
using SportHub.Services.Interfaces;
using SportHub.Views;
using System.Collections.Generic;
using SportHub.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text.RegularExpressions;

namespace SportHub.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IGetArticleService _articleService;
        private readonly ISearchService _searchArticles;

        public SearchModel(IGetArticleService articleService, ISearchService searchArticles)
        {
            _articleService = articleService;
            _searchArticles = searchArticles;
        }

        public IList<ArticleForSearchResult?> ArticlesSearch { get; set; }
        public IList<Article> Articles { get; set; }
        public string Title { get; set; }
        public int AmountOfFindedTitles { get; set; }
        public bool IsAdmin { get; set; }

        [Authorize(Roles = "Admin")]
        public void OnGetAuthorized(string? searchValue)
        {
            IsAdmin = true;
        }

        public void OnGet(string? searchValue)
        {
            Title = searchValue;
            AmountOfFindedTitles = _searchArticles.ArticleSearchAmount(searchValue);
            Articles = _searchArticles.ArticleSearchLimits(searchValue, 0, 10);
            ArticlesSearch = new List<ArticleForSearchResult>();
            for (int i = 0; i < Articles.Count; i++)
            {
                ArticleForSearchResult articleForSearchResult = new ArticleForSearchResult();
                articleForSearchResult.Id = Articles[i].Id;
                try
                {
                    articleForSearchResult.ContentText = Articles[i].ContentText;
                    articleForSearchResult.ContentText = Regex.Replace(articleForSearchResult.ContentText, "<.*?>", String.Empty);
                }
                catch
                {
                    articleForSearchResult.ContentText = "";
                }
                articleForSearchResult.Category = _articleService.GetArticlesCategory(Articles[i].Id);
                articleForSearchResult.Subcategory = _articleService.GetArticlesSubcategory(Articles[i].Id);
                articleForSearchResult.Team = _articleService.GetArticlesTeam(Articles[i].Id);
                ArticlesSearch.Add(articleForSearchResult);
            }
        }
    }
}