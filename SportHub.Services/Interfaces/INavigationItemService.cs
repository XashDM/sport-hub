using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface INavigationItemService
    {
        Task<NavigationItem> AddNewItem(NavigationItem Item);
        Task<List<NavigationItem>> GetChildrenOfItem(int ItemId);
        Task<List<NavigationItem>> GetTopCategories();
        Task<List<int>> GetRecusiveTree(int ItemId);
        Task<List<Article>> GetArticlesofItem(int ItemId);
        Task<bool> AddNewItems (List<NavigationItem> newItem);
    }
}
