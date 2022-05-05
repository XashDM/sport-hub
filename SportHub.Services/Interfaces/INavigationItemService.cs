using SportHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface INavigationItemService
    {
        public int AddNewItem(NavigationItem Item);
        public int DeletItem(int id);
        public int MoveItem(NavigationItem Item, NavigationItem ItemToMove);

        public List<NavigationItem> GetChildrenOfItem(int ItemId);
        public List<NavigationItem> GetRoute();
    }
}
