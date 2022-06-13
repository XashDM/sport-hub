using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
