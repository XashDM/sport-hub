using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class LikeDislikeCommentArgs
    {
        [Required]
        [Min(1)]
        public int MainCommentId { get; set; }

        [Required]
        [Min(1)]
        public int UserId { get; set; }

        [Required]
        public bool IsLiked { get; set; }
    }
}
