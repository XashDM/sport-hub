using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public int? ReferenceItemId { get; set; }
        public virtual NavigationItem? ReferenceItem { get; set; }
        public int ImageItemId { get; set; }
        public virtual ImageItem ImageItem { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string ContentText { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }
        public virtual ICollection<DisplayItem>? DisplayItems { get; set; }
        public bool IsPublished { get; set; }
    }
}

