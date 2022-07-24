using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class AccessTokenJwt
    {
        [Required]
        [MinLength(1)]
        public string Token { get; set; }
    }
}
