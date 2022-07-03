using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class EditedCommentArgs
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int MainCommentId { get; set; }
    }
}
