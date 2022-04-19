using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;
using SportHub.Services;

namespace SportHub.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IUserService _userService;

        public SignupModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public SignupCredentials signupCredentials { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost([FromBody] SignupCredentials credentials)
        {
            _userService.CreateUser(credentials.Email, credentials.PasswordHash, credentials.FirstName, credentials.LastName);

            return RedirectToPage("Index");
        }
    }
}
