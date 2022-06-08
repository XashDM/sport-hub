using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class ResetPassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        
    }
}
