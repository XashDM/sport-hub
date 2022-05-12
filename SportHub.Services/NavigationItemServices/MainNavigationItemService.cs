using Microsoft.EntityFrameworkCore;
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
        private readonly SportHubDBContext _context;
        private string[] Type = { "Category", "Subcategory", "Team" };

        public MainNavigationItemService(SportHubDBContext context)
        {
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
            catch (Exception ex)
            {
                return null;
            }
            return Item;
        }
        public async Task<List<NavigationItem>> GetChildrenOfItem(int ItemId)
        {
            var listOfChildren = await _context.NavigationItems
                .Where(navigationItem => navigationItem.ParentsItemId == ItemId)
                .ToListAsync();
            return listOfChildren;
        }
        public async Task<List<NavigationItem>> GetTopCategories()
        {
            var listOfChildren = await _context.NavigationItems
            .Where(navigationItem => navigationItem.ParentsItemId == null)
            .ToListAsync();
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

            var navigationItemIdList =await GetRecusiveTree(ItemId);
            var result = await _context.Articles.Where(articles => 
                navigationItemIdList.Contains(articles.ReferenceItemId.Value)).ToListAsync();
            return result;
        }
        public async Task<List<NavigationItem>> AddNewItems(List<NavigationItem> newItem)
        {
            foreach(var Item in newItem)
            {
                _context.NavigationItems.Add(Item);

            }
            await _context.SaveChangesAsync();
            return newItem;
        }
    }
}
