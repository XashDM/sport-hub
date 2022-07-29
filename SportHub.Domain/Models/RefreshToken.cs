using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportHub.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string JwtTokenId { get; set; }

        [Required]
        [Column(TypeName = "varchar(128)")]
        [MaxLength(128)]
        public string Token { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public bool IsRevoked { get; set; }

        [Required]
        public DateTime IssuedAt { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
