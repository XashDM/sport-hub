using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportHub.Domain;
using SportHub.Domain.Models;
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
        private string[] Type = { "Category", "Subcategory", "Team" };

        public MainNavigationItemService(ILogger<MainNavigationItemService> logger, SportHubDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<NavigationItem> AddNewItem(NavigationItem Item)
        {
            if (Type.Contains(Item.Name))
            {
                return null; 
            }
            try
            {
                _context.NavigationItems.Add(Item);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Sad message. I could not AddNewItem. I tried, but it happened: {e.Message}");
                return null;
            }

            return Item;
        }
        public async Task<List<NavigationItem>> GetChildrenOfItem(int ItemId)
        {
            List<NavigationItem> listOfChildren = null; 
            try
            {
                listOfChildren = await _context.NavigationItems
                .Where(navigationItem => navigationItem.ParentsItemId == ItemId)
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
        public async Task<List<int>> GetRecusiveTree(int ItemId)
        {
            var resUnion = _context.NavigationItems.Select(navigationItem => new NavigationItem
            {
                Id = navigationItem.Id,
                ParentsItemId = navigationItem.ParentsItemId,
                Name = navigationItem.Name,
                Type = navigationItem.Type
            })
            .Where(e => e.Id == ItemId);

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
        public async Task<List<Article>> GetArticlesofItem(int ItemId)
        {
            List<Article> result = null; 
            try
            {
                var navigationItemIdList = await GetRecusiveTree(ItemId);
                result = await _context.Articles.Where(articles =>
                navigationItemIdList.Contains(articles.ReferenceItemId.Value)).ToListAsync();
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Sad message. I could not GetArticlesofItem. I tried, but it happened: {e.Message}");
            }

            return result;
        }
        public async Task<bool> AddNewItems(List<NavigationItem> newItem)
        {
            bool isSaved = false;
            try
            {
                foreach (var Item in newItem)
                {
                    _context.NavigationItems.Add(Item);
                }
                _context.SaveChanges();

                isSaved = true;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Sad message. I could not save the data. I tried, but it happened: {e.Message}");
            }

            return isSaved;
        }
    }
}
