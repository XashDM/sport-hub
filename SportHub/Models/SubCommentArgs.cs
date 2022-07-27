using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class SubCommentArgs
    {
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
