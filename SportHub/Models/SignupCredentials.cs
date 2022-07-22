using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class SignupCredentials
    {
        [Display(Name = "First Name")]
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Such characters are not allowed.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Such characters are not allowed.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(64)]
        public string PasswordHash { get; set; }
    }
}
