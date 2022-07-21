namespace SportHub.Views
{
    public class ArticleForSearchResult
    {
        public int Id { get; set; }
        private string _contentText;

        public virtual string ContentText
        {
            get
            {
                return _contentText;
            }
            set
            {
                int length = 300;
                if (value.Length < length)
                {
                    _contentText = value;
                }
                else
                {
                    _contentText = value.Substring(0, length);
                }
            }
        }

        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public string? Team { get; set; }
    }
}