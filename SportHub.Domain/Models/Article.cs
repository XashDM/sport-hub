﻿using System;
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
        public string ImageLink { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ContentText { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }
        public virtual ICollection<DisplayItem>? DisplayItems { get; set; }
    }
}

