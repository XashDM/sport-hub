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
        private readonly IGetArticleService _getArticleService;

        public CommentService(SportHubDBContext context, IGetArticleService getArticleService)
        {
            _context = context;
            _getArticleService = getArticleService;
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

        public async Task<MainComment> EditComment(string message, int mainCommentId)
        {
            var foundComment = await _context.MainComments.Where(mainComment => mainComment.Id == mainCommentId).FirstOrDefaultAsync();

            foundComment.Message = message;
            foundComment.Created = DateTime.UtcNow;
          
            await _context.SaveChangesAsync();
            return foundComment;
        }

        public IQueryable<MainComment> GetSortedComments(string sortedBy, int articleId)
        {
            var mainComments = _context.MainComments
                .Where(comment => comment.ArticleId.Equals(articleId));

            if (sortedBy == "popular")
            {
                return mainComments.OrderByDescending(comment => comment.Likes);
            }
            else if (sortedBy == "newest")
            {
                return mainComments.OrderByDescending(date => date.Created);
            }
            else
            {
                return mainComments.OrderBy(date => date.Created);
            }
        }

        public async Task<(MainComment[], int)> GetSortedCommentPaginatedAsync(string sortedBy, int articleId, int pageSize, int pageNumber) 
        {
            var sortedMainComments = GetSortedComments(sortedBy, articleId);
            var paginationResult = _getArticleService.Paginate(sortedMainComments, pageSize, pageNumber);

            var paginatedMainComments = await paginationResult.Item1
                .Include(comment => comment.User)
                .ToArrayAsync();

            var totalComments = paginationResult.Item3;

            return (paginatedMainComments, totalComments);
        }

        public async Task<CommentUserLikeDislike?> LikeOrDislikeComment(int mainCommentId, int userId, bool isLiked)
        {
            var record = await _context.CommentUserLikeDislikes
                                            .Where(entry => entry.MainCommentId.Equals(mainCommentId) && entry.UserId.Equals(userId))
                                            .SingleOrDefaultAsync();
            
            var comment = await _context.MainComments.Where(comment => comment.Id.Equals(mainCommentId)).SingleOrDefaultAsync();

            if (record == null)
            {
                var likedComment = new CommentUserLikeDislike
                {
                    UserId = userId,
                    MainCommentId = mainCommentId,
                    IsLiked = isLiked,
                };

                await _context.CommentUserLikeDislikes.AddAsync(likedComment);
                await _context.SaveChangesAsync();

                if (isLiked)
                {
                    comment.Likes += 1;
                }
                else
                {
                    comment.Dislikes += 1;
                }

                await _context.SaveChangesAsync();

                return likedComment;
            }
            else
            {
                if (isLiked)
                {
                    if (record.IsLiked)
                    {
                        _context.CommentUserLikeDislikes.Remove(record);
                        
                        await _context.SaveChangesAsync();

                        comment.Likes -= 1;

                        await _context.SaveChangesAsync();

                        return null;
                    }
                    else
                    {
                        record.IsLiked = true;
                        await _context.SaveChangesAsync();

                        comment.Likes += 1;
                        comment.Dislikes -= 1;

                        await _context.SaveChangesAsync();

                        return record;
                    }
                }
                else
                {
                    if (record.IsLiked)
                    {
                        record.IsLiked = false;
                        await _context.SaveChangesAsync();

                        comment.Likes -= 1;
                        comment.Dislikes += 1;

                        await _context.SaveChangesAsync();

                        return record;
                    }
                    else
                    {
                        _context.CommentUserLikeDislikes.Remove(record);
                        await _context.SaveChangesAsync();

                        comment.Dislikes -= 1;

                        await _context.SaveChangesAsync();

                        return null;
                    }
                }
            }
        }

        public SubComment CreateSubComment(int mainCommentId, string message, int userId)
        {
            var newSubComment = new SubComment
            {
                UserId = userId,
                MainCommentId = mainCommentId,
                Message = message,
                Created = DateTime.UtcNow,
            };
            _context.SubComments.Add(newSubComment);
            _context.SaveChanges();
            return newSubComment;
        }
    }
}
