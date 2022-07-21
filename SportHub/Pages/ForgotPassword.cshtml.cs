using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SportHub.Config.JwtAuthentication;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Interfaces;

namespace SportHub.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        private readonly IJwtSigner _jwtSigner;
        private readonly IEmailService _emailService;
        private readonly IFluentEmail _mailer;

        [BindProperty]
        public ForgotPassword forgotPassword { get; set; }

        public ForgotPasswordModel(ILogger<LoginModel> logger, IUserService userService, IJwtSigner jwtSigner, 
                                               IEmailService emailService, [FromServices] IFluentEmail mailer)
        {
            _logger = logger;
            _userService = userService;
            _jwtSigner = jwtSigner;
            _emailService = emailService;
            _mailer = mailer;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(ForgotPassword forgotPassword)
        {
            if (!_userService.IsExistingEmail(forgotPassword.Email))
            {
                return BadRequest("User with such email doesn't exist");
            }

            var currentUser = _userService.GetUserByEmail(forgotPassword.Email);
            var token = _jwtSigner.FetchToken(currentUser);

            _emailService.SendResetPasswordEmail(currentUser, _mailer, token.TokenJwt);

            return StatusCode(200);
        }
    }
}