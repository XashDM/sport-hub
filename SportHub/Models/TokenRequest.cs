using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class TokenRequest
    {
        [Required]
        [MinLength(1)]
        public string AccessToken { get; set; }

        [Required]
        [StringLength(128)]
        public string RefreshToken { get; set; }
    }
}
