using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class LoginCredentials
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
