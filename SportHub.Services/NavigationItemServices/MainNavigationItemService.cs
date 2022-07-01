using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Domain.ViewModel;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Services.NavigationItemServices
{
    public class MainNavigationItemService : INavigationItemService
    {
        private readonly ILogger<MainNavigationItemService> _logger;
        private readonly SportHubDBContext _context;

        public MainNavigationItemService(ILogger<MainNavigationItemService> logger, SportHubDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<NavigationItem>> GetChildrenOfItem(int itemId)
        {
            List<NavigationItem> listOfChildren = null; 
            try
            {
                listOfChildren = await _context.NavigationItems
                .Where(navigationItem => navigationItem.ParentsItemId == itemId)
                .ToListAsync();
            }
            catch(System.Exception e)
            {
                _logger.LogError($"Sad message. I could not GetChildrenOfItem. I tried, but it happened: {e.Message}");
            }

            return listOfChildren;
        }

        public async Task<List<NavigationItem>> GetTopCategories()
        {
            List<NavigationItem> listOfChildren = null;
            try
            {
                listOfChildren = await _context.NavigationItems
                .Where(navigationItem => navigationItem.ParentsItemId == null)
                .ToListAsync();
            }
            catch(System.Exception e)
            {
                _logger.LogError($"Sad message. I could not GetTopCategories. I tried, but it happened: {e.Message}");
            }

            return listOfChildren;
        }

        public async Task<List<int>> GetRecusiveTree(int itemId)
        {
            var resUnion = _context.NavigationItems.Select(navigationItem => new NavigationItem
            {
                Id = navigationItem.Id,
                ParentsItemId = navigationItem.ParentsItemId,
                Name = navigationItem.Name,
                Type = navigationItem.Type
            })
            .Where(e => e.Id == itemId);

            int maxLevel = 3;
            int level = 1;
            var res = resUnion;
            while (level <= maxLevel)
            {
                res = res.Join(_context.NavigationItems,
                first => first.Id,
                second => second.ParentsItemId,
                (first, second) => new NavigationItem
                {
                    Id = second.Id,
                    ParentsItemId = second.ParentsItemId,
                    Name = second.Name,
                    Type = second.Type,
                });
                level += 1;
                resUnion = resUnion.Union(res);
            }

            return await resUnion.Select(navigationItem => navigationItem.Id).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesofItem(int itemId)
        {
            List<Article> result = null; 
            try
            {
                var navigationItemIdList = await GetRecusiveTree(itemId);
                result = await _context.Articles.Where(articles =>
                navigationItemIdList.Contains(articles.ReferenceItemId.Value)).ToListAsync();
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Sad message. I could not GetArticlesofItem. I tried, but it happened: {e.Message}");
            }

            return result;
        }

        private async Task<bool> SaveItems(List<NavigationItemForSave>? newItems)
        {
            List<NavigationItem> navigationItems = new List<NavigationItem>(); ;
            foreach (var Item in newItems)
            {
                NavigationItem saveItem = new NavigationItem
                {
                    Name = Item.Name,
                    Type = Item.Type,
                    ParentsItemId = Item.ParentsItemId
                };
                navigationItems.Add(saveItem);
            }
            _context.NavigationItems.AddRange(navigationItems);
            _context.SaveChanges();
            for (int i = 0; i < navigationItems.Count ; i++)
            {
                foreach (var item in newItems[i].Children)
                {
                    item.ParentsItemId = navigationItems[i].Id;
                }
                SaveItems(newItems[i].Children);
            }
            return true;
        }

        public async Task<bool> AddNewItems(List<NavigationItemForSave> newItems)
        {
            bool isSaved = false;
            try
            {
                isSaved = await SaveItems(newItems);
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Sad message. I could not save the data. I tried, but it happened: {e.Message}");
            }

            return isSaved;
        }
    }
}
