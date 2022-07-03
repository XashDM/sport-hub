using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services
{
    public class CommentService : ICommentService
    {
        private readonly SportHubDBContext _context;

        public CommentService(SportHubDBContext context)
        {
            _context = context;
        }

        public void DeleteComment(int mainCommentId)
        {
            var commentToDelete = _context.MainComments.Where(mainComment => mainComment.Id.Equals(mainCommentId)).Single();
            _context.MainComments.Remove(commentToDelete);
            _context.SaveChanges();
        }

        public async Task<int> GetCommentCount(int articleId)
        {
            var commentCount = await _context.MainComments.Where(mainComment => mainComment.ArticleId == articleId).CountAsync();
            return commentCount;
        }

        public async Task<MainComment[]> GetComments (int articleId)
        {
            var mainComments = await _context.MainComments.Where(comment => comment.ArticleId == articleId)
                .Include(comment => comment.User)
                .ToArrayAsync();
            
            return mainComments;
        }

        public async Task<MainComment> EditComment(string message, int mainCommentId)
        {
            var foundComment = await _context.MainComments.Where(mainComment => mainComment.Id == mainCommentId).FirstOrDefaultAsync();

            foundComment.Message = message;
            foundComment.Created = DateTime.UtcNow;
          
            await _context.SaveChangesAsync();
            return foundComment;
        }

        public async Task<MainComment[]> GetSortedComments(string sortedBy)
        {
            if (sortedBy == "popular")
            {
                var mainComments = await _context.MainComments.OrderByDescending(comment => comment.Likes)
                .ToArrayAsync();
                return mainComments;
            }
            else if (sortedBy == "newest")
            {
                var mainComments = await _context.MainComments.OrderByDescending(date => date.Created)
                .ToArrayAsync();
                return mainComments;
            }
            else
            {
                var mainComments = await _context.MainComments.OrderBy(date => date.Created)
                .ToArrayAsync();
                return mainComments;
            }
        }
    }
}
