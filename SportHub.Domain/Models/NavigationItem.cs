using System.Collections.Generic;
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
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public int? ParentsItemId { get; set; }
        public virtual NavigationItem? ParentsItem { get; set; }
        [JsonIgnore]
        public ICollection<NavigationItem> Children { get; } = new List<NavigationItem>();
    }
}
