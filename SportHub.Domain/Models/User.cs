using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportHub.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }

        [MaxLength(64)]
        [Column(TypeName = "char(64)")]
        [JsonIgnore]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(320)]
        [Column(TypeName = "varchar(320)")]
        [EmailAddress]
        [JsonIgnore]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool IsActive { get; set; }

        [Required]
        public bool IsExternal { get; set; }

        [Column(TypeName = "varchar(20)")]
        [MaxLength(50)]
        public string? AuthProvider { get; set; } // Google, Facebook, etc. 

        public virtual ICollection<UserRole> Roles { get; set; }
        
        public virtual ICollection<CommentUserLikeDislike> LikesDislikes { get; set; }
    }
}
