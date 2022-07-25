namespace SportHub.Models
{
    public class SortedMainCommentsArgs
    {
        public PageArgs PageArgs { get; set; }

        public string SortedBy { get; set; }

        public int ArticleId { get; set; }
    }
}
