using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services.NavigationItemServices
{
    public class MainNavigationItemService : INavigationItemService
    {
        private readonly SportHubDBContext _context;

        public MainNavigationItemService(SportHubDBContext context)
        {
            _context = context;
        }
        private string[] Type = {"Category", "Subcategory", "Team"};

        public int AddNewItem(NavigationItem Item)
       {
            /**/
            _context.NavigationItems.Add(Item);
            _context.SaveChanges();

            return 0;
       }
        public int DeletItem(int id)
        {
            return 0;
        }
        public int MoveItem(NavigationItem Item, NavigationItem ItemToMove)
        {
            return 0;
        }

        public List<NavigationItem> GetChildrenOfItem(int ItemId)
        {
            var listOfCheldren = _context.NavigationItems
                .Where(NI => NI.ParentItemId == ItemId)
                .ToList();
            return listOfCheldren;
        }
        public List<NavigationItem> GetRoute()
        {
            var listOfCheldren = _context.NavigationItems
                .Where(NI => NI.ParentItemId == null)
                .ToList();
            return listOfCheldren;
        }
    }
}
