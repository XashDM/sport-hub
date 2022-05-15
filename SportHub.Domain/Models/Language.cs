using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string LanguageName { get; set; }

        public bool IsEnabled { get; set; }

        public static implicit operator List<object>(Language v)
        {
            throw new NotImplementedException();
        }
    }
}

