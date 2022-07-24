using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Interfaces;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;
        private readonly IEmailService _emailService;

        [BindProperty]
        public ForgotPassword forgotPassword { get; set; }

        public ForgotPasswordModel(ILogger<LoginModel> logger, IUserService userService, IJwtSigner jwtSigner, IEmailService emailService)
        {
            _logger = logger;
            _userService = userService;
            _jwtSigner = jwtSigner;
            _emailService = emailService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(ForgotPassword forgotPassword)
        {
            if (!_userService.IsExistingEmail(forgotPassword.Email))
            {
                return BadRequest("User with such email doesn't exist");
            }

            var currentUser = _userService.GetUserByEmail(forgotPassword.Email);
            var token = _jwtSigner.FetchToken(currentUser);

            await _emailService.SendResetPasswordEmail(currentUser, token.TokenJwt);

            return StatusCode(200);
        }
    }
}