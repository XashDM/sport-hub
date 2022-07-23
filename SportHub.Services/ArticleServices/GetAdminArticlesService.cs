using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SportHub.Services.NavigationItemServices;
using SportHub.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SportHub.Services.ArticleServices
{
    public class GetAdminArticlesService : IGetAdminArticlesService
    {
        private readonly SportHubDBContext _context;
        public GetAdminArticlesService(SportHubDBContext context)
        {
            _context = context;
        }

        public IList<Article> GetArticlesRange(int start, int end, string? publishValue, string? category, string? subcategory, string? team, string? search)
        {
            IList<Article> articles = GetArticlesBySearch(publishValue, category, subcategory, team, search).Skip(start).Take(end).ToList();
            return articles;
        }

        public IList<Article> GetArticlesBySearch(string? publishValue, string? category, string? subcategory, string? team, string? search)
        {
            IList<Article> articles = GetArticlesByPublished(publishValue, category, subcategory, team);
            IList<Article> articlesSearched = new List<Article>();
            
            if (search != null)
            {
                for (int i = 0; i < articles.Count; i++)
                {
                    if (articles[i].ContentText.Contains(search) || articles[i].Title.Contains(search))
                    {
                        articlesSearched.Add(articles[i]);
                    }
                }
                return articlesSearched;
            }

            return articles;
        }

        public IList<Article> GetArticlesByPublished(string? publishValue, string? category, string? subcategory, string? team)
        {
            IList<Article> articles = GetArticles(category, subcategory, team);
            IList<Article> publishedArticles = new List<Article>();
            if(publishValue != "All" && publishValue != null && publishValue != "")
            {
                for(int i = 0; i < articles.Count; i++)
                {
                    if(publishValue == "Published" && articles[i].IsPublished == true)
                    {
                        publishedArticles.Add(articles[i]);
                    }
                    if (publishValue == "Unpublished" && articles[i].IsPublished == false)
                    {
                        publishedArticles.Add(articles[i]);
                    }
                }
                return publishedArticles;
            }
            return articles;
            
        }
        public IList<Article> GetArticles(string? category, string? subcategory, string? team)
        {
            try
            {
                IList<Article> articles = new List<Article>();
                IList<NavigationItem> navigationItems = new List<NavigationItem>();
                IList<NavigationItem> allTeamsItem = new List<NavigationItem>();
                NavigationItem categoryItem = _context.NavigationItems.FirstOrDefault(item => item.Name.Equals(category) && item.Type.Equals("Category"));
                if (categoryItem != null)
                {
                    if (team != null && subcategory != null)
                    {
                        NavigationItem subcategoryItem = _context.NavigationItems.FirstOrDefault(item => item.Name.Equals(subcategory) && item.ParentsItemId.Equals(categoryItem.Id));
                        if (subcategoryItem != null)
                        {
                            navigationItems = _context.NavigationItems.Where(item => item.Name.Equals(team) && item.Type.Equals("Team") && item.ParentsItemId.Equals(subcategoryItem.Id)).ToList();
                            if (subcategoryItem != null)
                            {
                                for (int i = 0; i < navigationItems.Count; i++)
                                {
                                    articles.AddRange(_context.Articles
                                        .Where(article => article.ReferenceItemId.Equals(navigationItems[i].Id))
                                        .Include(article => article.ImageItem)
                                        .ToList());
                                }
                                return articles;
                            }
                        }    
                    }
                    else
                    {
                        if (subcategory == null)
                        {
                            IList<NavigationItem> subcategoryItems = new List<NavigationItem>();
                            subcategoryItems = _context.NavigationItems.Where(items => items.ParentsItemId.Equals(categoryItem.Id)).ToList();
                            articles.AddRange(_context.Articles
                                .Where((article) => article.ReferenceItemId.Equals(categoryItem.Id))
                                .Include(article => article.ImageItem)
                                .ToList());
                            for (int i = 0; i < subcategoryItems.Count; i++)
                            {
                                articles.AddRange(_context.Articles
                                    .Where((article) => article.ReferenceItemId.Equals(subcategoryItems[i].Id))
                                    .Include(article => article.ImageItem)
                                    .ToList());
                            }
                            for (int i = 0; i < subcategoryItems.Count; i++)
                            {
                                allTeamsItem.AddRange(_context.NavigationItems.Where(items => items.ParentsItemId.Equals(subcategoryItems[i].Id)));

                            }
                            for (int i = 0; i < allTeamsItem.Count; i++)
                            {
                                articles.AddRange(_context.Articles
                                    .Where(article => article.ReferenceItemId.Equals(allTeamsItem[i].Id))
                                    .Include(article => article.ImageItem)
                                    .ToList());
                            }
                            return articles;
                        }
                        else
                        {
                            navigationItems = _context.NavigationItems.Where(item => item.Name.Equals(subcategory)
                                && item.Type.Equals("Subcategory")
                                && item.ParentsItemId.Equals(categoryItem.Id)).ToList();
                            for (int i = 0; i < navigationItems.Count; i++)
                            {
                                articles.AddRange(_context.Articles
                                    .Where(item => item.ReferenceItemId.Equals(navigationItems[i].Id))
                                    .Include(article => article.ImageItem)
                                    .ToList());
                                allTeamsItem.AddRange(_context.NavigationItems.Where(item => item.ParentsItemId == navigationItems[i].Id).ToList());
                            }
                            for (int i = 0; i < allTeamsItem.Count; i++)
                            {
                                articles.AddRange(_context.Articles
                                    .Where(article => article.ReferenceItemId.Equals(allTeamsItem[i].Id))
                                    .Include(article => article.ImageItem)
                                    .ToList());
                            }
                            return articles;
                        }
                    }
                    articles = _context.Articles.Include(article => article.ImageItem).ToList();
                    return articles;
                }
                else
                {
                    articles = _context.Articles.Include(article => article.ImageItem).ToList();
                    for (int i = 0; i<articles.Count; i++)
                    {
                        articles[i].ReferenceItem = _context.NavigationItems.FirstOrDefault(item => item.Id.Equals(articles[i].ReferenceItemId));
                    }
                    return articles;
                }
            }
            catch
            {
                return null;
            }
        }

        public IList<NavigationItem> GetCategories()
        {
            return _context.NavigationItems.Where(item => item.Type.Equals("Category")).ToList();
        }

        public IList<NavigationItem> GetCategoriesToMove(string? category)
        {
            IList<NavigationItem> categoriesToMove = GetCategories();
            if (category != null)
            {
                categoriesToMove.Remove(categoriesToMove.First(item => item.Name.ToLower() == category.ToLower()));
            }
            return categoriesToMove;
        }

        public IList<NavigationItem> GetSubcategories(string category)
        {
            try
            {
                NavigationItem categoryItem = new NavigationItem();
                categoryItem = _context.NavigationItems.FirstOrDefault(navigationItem => navigationItem.Name.Equals(category) && navigationItem.Type == "Category");
                if (categoryItem != null)
                {
                    IList<NavigationItem> subcategories = _context.NavigationItems.Where(navigationItem => navigationItem.ParentsItemId.Equals(categoryItem.Id)).ToList();
                    if (subcategories.Count > 0)
                    {
                        return subcategories;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public IList<NavigationItem> GetTeams(string? subcategory)
        {
            try
            {
                NavigationItem subcategoryItem = new NavigationItem();
                subcategoryItem = _context.NavigationItems.FirstOrDefault(navigationItem => navigationItem.Name.Equals(subcategory) && navigationItem.Type == "Subcategory");
                if (subcategoryItem != null)
                {
                    IList<NavigationItem> teams = _context.NavigationItems.Where(navigationItem => navigationItem.ParentsItemId.Equals(subcategoryItem.Id)).ToList();
                    if (teams.Count > 0)
                    {
                        return teams;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
