using System;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Domain.Models
{
    public class SubComment
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public int MainCommentId { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public virtual User User { get; set; }
        public virtual MainComment MainComment { get; set; }
    }
}