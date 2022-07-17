using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class MainArticlesToSaveArgs
    {
        [Required]
        public Dictionary<int, bool> ArticlesDisplayValues { get; set; }
    }
}
