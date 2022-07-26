using SportHub.Domain.Models;

namespace SportHub.Models
{
    public class SortedMainCommentsOut
    {
        public MainComment[] Comments { get; set; }
        public int CommentCount { get; set; }
    }
}
