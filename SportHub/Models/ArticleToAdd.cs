using Microsoft.AspNetCore.Http;
using SportHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class ArticleToAdd
    {
        public int? ReferenceItemId { get; set; }
        public string AlternativeTextForThePicture { get; set; }
        public string Title { get; set; }
        public IFormFile? imageFile { get; set; }
        public string ShortDescriptionOfThePicture { get; set; }
        public string ContentText { get; set; }
    }
}

