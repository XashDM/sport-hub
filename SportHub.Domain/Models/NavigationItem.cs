using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<NavigationItem> Children { get; } = new List<NavigationItem>();
    }
}
