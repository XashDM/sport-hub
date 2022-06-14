using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Domain.Models
{
    public class DisplayItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; } // hardcoded for now
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string DisplayLocation { get; set; } // hardcoded for now
        [Required]
        public bool IsDisplayed { get; set; }
        public int? ArticleId { get; set; }
        public virtual Article? Article { get; set; }
        public int? ImageItemId { get; set; }
        public virtual ImageItem? ImageItem { get; set; }
    }
}
