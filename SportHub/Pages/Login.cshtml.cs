using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using System;

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
            try
            {
                var currentUser = _userService.GetUserByEmail(loginCredentials.Email);

                if (currentUser.IsExternal)
                {
                    return BadRequest("Cannot explicitly authorize externally added user");
                }

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
            catch (UserServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
