using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly SportHubDBContext _context;
        public IList<User> users { get; set; }
        public IList<User> admins { get; set; }

        public UserManagementModel(SportHubDBContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            users = _context.Users.Include(u => u.Roles).ToList();

            var userRole = _context.UserRoles
                .Where(r => r.RoleName == "Admin")
                .Include(r => r.Users)
                .ToList();

            admins = userRole[0].Users.ToList();
        }
    }
}
