using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportHub.Domain.Models
{
    public class NavigationItem

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Type { get; set; }
        public int? ParentsItemId { get; set; }
        public virtual NavigationItem? ParentsItem { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<NavigationItem>? Children { get; set; }
    }
}
