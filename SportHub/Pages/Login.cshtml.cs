using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Config.JwtAuthentication;
using Microsoft.EntityFrameworkCore;
using SportHub.Models;
using SportHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using SportHub.Domain.Models;

namespace SportHub.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;

        [BindProperty]
        public LoginCredentials loginCredentials { get; set; }
        public LoginModel(ILogger<LoginModel> logger, IUserService userService, IJwtSigner jwtSigner)
        {
            _logger = logger;
            _userService = userService;
            _jwtSigner = jwtSigner;
           
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(LoginCredentials loginCredentials)
        { 
            if (!_userService.IsExistingEmail(loginCredentials.Email))
            {
                return BadRequest("Invalid email");
            }

            var currentUser = _userService.GetUserByEmail(loginCredentials.Email);

            if (ModelState.IsValid)
            {
                if (!loginCredentials.PasswordHash.Equals(currentUser.PasswordHash))
                {
                    return BadRequest("Invalid password");
                }
                
            }
              var token = _jwtSigner.FetchToken(currentUser);
              return Content(token);
        }
    }
}
