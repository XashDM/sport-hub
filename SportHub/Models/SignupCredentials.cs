using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class SignupCredentials
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
    }
}
