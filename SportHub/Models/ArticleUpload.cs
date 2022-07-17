using SportHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class ArticleUpload
    {   
        public int Id { get; set; }
        public int? ReferenceItemId { get; set; }
        public virtual NavigationItem? ReferenceItem { get; set; }
        public string AlternativeTextForThePicture { get; set; }
        public int ImageItemId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ShortDescriptionOfThePicture { get; set; }
        [Required]
        public string ContentText { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }
        public bool IsPublished { get; set; }
    }
}

