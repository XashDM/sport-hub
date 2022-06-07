using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class PhotoOfTheDayModel
    {
        
        public IFormFile? imageFile { get; set; }
        public string Alt { get; set; }
        [Required]
        public string PhotoTitle { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Author { get; set; }
    }
}
