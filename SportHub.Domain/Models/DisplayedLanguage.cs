using System;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class DisplayedLanguage
    {
        [Key]
        public int Id { get; set; }

        public string LanguageName { get; set; }

        public bool IsEnabled { get; set; }
    }
}

