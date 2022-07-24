using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SportHub.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ResetPasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }
    }
}