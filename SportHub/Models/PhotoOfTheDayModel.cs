using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class PhotoOfTheDayModel
    {
        
        public IFormFile? imageFile { get; set; }
        [MaxLength(100)]
        public string Alt { get; set; }
        [Required]
        [MaxLength(50)]
        public string PhotoTitle { get; set; }
        [Required]
        [MaxLength(50)]
        public string ShortDescription { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        public bool? isDisplayed { get; set; }
    }
}
