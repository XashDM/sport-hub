using SportHub.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ICommentService
    {
        public void DeleteComment(int mainCommentId);
        public Task<int> GetCommentCount(int articleId);
        public Task<MainComment> EditComment(string message, int mainCommentId);
        Task<CommentUserLikeDislike> LikeOrDislikeComment(int mainCommentId, int userId, bool isLiked);
        IQueryable<MainComment> GetSortedComments(string sortedBy, int articleId);
        Task<(MainComment[], int)> GetSortedCommentPaginatedAsync(string sortedBy, int articleId, int pageSize, int pageNumber);
    }
}
