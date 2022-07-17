using DataAnnotationsExtensions;

namespace SportHub.Models
{
    public class PageArgs
    {
        private const int maxPageSize = 20;

        [Min(1)]
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        [Min(1)]
        public int PageSize { get { return _pageSize; } set { _pageSize = (value > maxPageSize) ? maxPageSize : value; } }
    }
}
