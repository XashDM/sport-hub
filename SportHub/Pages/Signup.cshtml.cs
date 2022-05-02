using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;

namespace SportHub.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IFluentEmail _mailer;

        public SignupModel(IUserService userService, IEmailService emailService, [FromServices] IFluentEmail mailer)
        {
            _userService = userService;
            _emailService = emailService;
            _mailer = mailer;
        }
        [BindProperty]
        public SignupCredentials signupCredentials { get; set; }

        public IActionResult OnPost(SignupCredentials credentials)
        {
            try
            {
                var user = _userService.CreateUser(credentials.Email, credentials.PasswordHash, credentials.FirstName, credentials.LastName);
                _emailService.SendSignUpEmail(user, _mailer);

                Response.StatusCode = 201;
                return new JsonResult(new { success = true });
            }
            catch (UserServiceException e)
            {
                Response.StatusCode = e.StatusCode;
                return new JsonResult(new { error = e.Message });
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return new JsonResult(new { error = "Something went wrong" });
            }
        }
    }
}
