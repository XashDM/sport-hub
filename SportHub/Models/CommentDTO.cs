using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "asdasdas")]
        public int ArticleId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
