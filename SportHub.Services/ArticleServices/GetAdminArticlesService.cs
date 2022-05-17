using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SportHub.Services.NavigationItemServices;
using FluentEmail.Core;

namespace SportHub.Services.ArticleServices
{
    public class GetAdminArticlesService : IGetAdminArticlesService
    {
        private readonly SportHubDBContext _context;
        public GetAdminArticlesService(SportHubDBContext context)
        {
            _context = context;
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
                                    articles.AddRange(_context.Articles.Where(article => article.ReferenceItemId.Equals(navigationItems[i].Id)).ToList());
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
                            articles.AddRange(_context.Articles.Where((article) => article.ReferenceItemId.Equals(categoryItem.Id)).ToList());
                            for (int i = 0; i < subcategoryItems.Count; i++)
                            {
                                articles.AddRange(_context.Articles.Where((article) => article.ReferenceItemId.Equals(subcategoryItems[i].Id)).ToList());
                            }
                            for (int i = 0; i < subcategoryItems.Count; i++)
                            {
                                allTeamsItem.AddRange(_context.NavigationItems.Where(items => items.ParentsItemId.Equals(subcategoryItems[i].Id)));

                            }
                            for (int i = 0; i < allTeamsItem.Count; i++)
                            {
                                articles.AddRange(_context.Articles.Where(article => article.ReferenceItemId.Equals(allTeamsItem[i].Id)).ToList());
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
                                articles.AddRange(_context.Articles.Where(item => item.ReferenceItemId.Equals(navigationItems[i].Id)).ToList());
                                allTeamsItem.AddRange(_context.NavigationItems.Where(item => item.ParentsItemId == navigationItems[i].Id).ToList());
                            }
                            for (int i = 0; i < allTeamsItem.Count; i++)
                            {
                                articles.AddRange(_context.Articles.Where(article => article.ReferenceItemId.Equals(allTeamsItem[i].Id)).ToList());
                            }
                            return articles;
                        }
                    }
                    articles = _context.Articles.ToList();
                    return articles;
                }
                else
                {
                    return _context.Articles.ToList();
                }
            }
            catch
            {
                return null;
            }
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
