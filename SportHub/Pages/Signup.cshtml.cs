using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;
using SportHub.Services;

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
        public void OnGet()
        {
        }

        public void OnPost(SignupCredentials credentials)
        {
            //var user = _userService.CreateUser(credentials.Email, credentials.PasswordHash, credentials.FirstName, credentials.LastName);
            var user = _userService.GetUserByEmail(credentials.Email);
            _emailService.SendSignUpEmail(user);
        }
    }
}
