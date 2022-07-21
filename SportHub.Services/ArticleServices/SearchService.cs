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
    public class SearchService : ISearchService
    {
        private readonly SportHubDBContext _context;

        public SearchService(SportHubDBContext context)
        {
            _context = context;
        }

        public IList<Article> ArticleSearch(string search)
        {
            IList<Article> articles = new List<Article>();
            articles = _context.Articles.Where(article => (article.Title.Contains(search) || article.ContentText.Contains(search)) && article.IsPublished.Equals(true)).ToList();
            return articles;
        }

        public IList<Article> ArticleSearchLimits(string search, int startPosition, int endPosition)
        {
            var articles = ArticleSearch(search);
            return articles.Skip(startPosition).Take(endPosition).ToList();
        }

        public IQueryable<Article> ArticleSearchAllTree(string search)
        {
            var articles = _context.Articles
                .AsNoTracking()
                .Include(article => article.ReferenceItem)
                .ThenInclude(refItem => refItem.ParentsItem)
                .ThenInclude(refItem => refItem.ParentsItem)
                .Where(article => (article.Title.Contains(search) || article.ContentText.Contains(search)) && article.IsPublished.Equals(true));

            return articles;
        }

        public int ArticleSearchAmount(string search)
        {
            var articleAmount = _context.Articles.Where(article => (article.Title.Contains(search) || article.ContentText.Contains(search)) && article.IsPublished.Equals(true)).Count();
            return articleAmount;
        }
    }
}