using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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

            var adminRole = _context.UserRoles
                .Where(role => role.RoleName == "Admin")
                .Include(role => role.Users)
                .ToList();

            admins = adminRole[0].Users.ToList();
        }
    }
}
