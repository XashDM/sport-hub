using DataAnnotationsExtensions;

namespace SportHub.Models
{
    public class PageArgs
    {
        [Min(1)]
        public int PageNumber { get; set; } = 1;

        [Min(1)]
        public int PageSize { get { return _pageSize; } set { _pageSize = (value > maxPageSize) ? maxPageSize : value; } }

        private const int maxPageSize = 20;
        private int _pageSize = 10;
    }
}
