using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportHub.Models;

namespace SportHub.Pages
{
    public class SignupModel : PageModel
    {
        [BindProperty]
        public SignupCredentials signupCredentials { get; set; }
        public void OnGet()
        {
        }
    }
}
