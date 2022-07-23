using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using System;
using System.Threading.Tasks;

namespace SportHub.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public SignupModel(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        [BindProperty]
        public SignupCredentials signupCredentials { get; set; }

        public async Task<IActionResult> OnPost(SignupCredentials credentials)
        {
            try
            {
                var user = await _userService.CreateUser(credentials.Email, credentials.PasswordHash, credentials.FirstName, credentials.LastName);
                await _emailService.SendSignUpEmail(user);

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
                return new JsonResult(new { error = "Something went wrong. Please try again later" });
            }
        }
    }
}
