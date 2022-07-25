using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class CommentUserLikeDislike
    {
        public int Id { get; set; }

        [Required]
        public bool IsLiked { get; set; }

        public int UserId { get; set; }

        public int MainCommentId { get; set; }

        public virtual User User { get; set; }

        public virtual MainComment MainComment { get; set; }
    }
}
