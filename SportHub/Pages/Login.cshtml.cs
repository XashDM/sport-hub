using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Models.Output;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;
        private readonly ITokenService _tokenService;

        [BindProperty]
        public LoginCredentials loginCredentials { get; set; }
        public LoginModel(ILogger<LoginModel> logger, IUserService userService, IJwtSigner jwtSigner, ITokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _jwtSigner = jwtSigner;
            _tokenService = tokenService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(LoginCredentials loginCredentials)
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

                var accessToken = _jwtSigner.FetchToken(currentUser);
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken.Id, currentUser.Id);

                return new JsonResult(new AuthTokenResponse() { AccessToken = accessToken.TokenJwt, RefreshToken = refreshToken.Token });
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
