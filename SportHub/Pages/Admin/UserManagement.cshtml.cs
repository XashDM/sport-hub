using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services;
using System.Collections.Generic;

namespace SportHub.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly SportHubDBContext _context;
        private readonly IUserService _userService;
        public IList<User> Users { get; set; }
        public IList<User> Admins { get; set; }
        public bool IsAdminTable { get; set; }
        public UserManagementModel(SportHubDBContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void OnGet(string? accessType)
        {
            Admins = _userService.GetAllAdminsList();

            if (accessType is null)
            {
                Users = _userService.GetAllUsersList();
            }
            else
            {
                if (accessType == "admin")
                {
                    IsAdminTable = true;
                }
                else
                {
                    Users = _userService.GetAllUsersList();
                }
            }
        }
    }
}
