using System;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public int ReferenceItemId { get; set; }
        public virtual NavigationItem? ReferenceItem { get; set; }
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }
    }
}

