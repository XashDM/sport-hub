using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SportHub.Services.NavigationItemServices;
using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;

namespace SportHub.Services.ArticleServices
{
    public class SearchArticles : ISearchArticles
    {
        private readonly SportHubDBContext _context;
        public SearchArticles(SportHubDBContext context)
        {
            _context = context;
        }
        public IList<Article> ArticlesBySearch(string search)
        {
            IList<Article> articles = new List<Article>();
            articles = _context.Articles.Where(article => (article.Title.Contains(search) || article.ContentText.Contains(search)) && article.IsPublished.Equals(true)).ToList();
            return articles;
        }

        public IList<Article> ArticlesBySearchRange(string search, int startPosition, int endPosition)
        {
            var articles = ArticlesBySearch(search);
            return articles.Skip(startPosition).Take(endPosition).ToList();
        }
    }
}