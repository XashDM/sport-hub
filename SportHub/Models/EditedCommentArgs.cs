using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class EditedCommentArgs
    {
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}