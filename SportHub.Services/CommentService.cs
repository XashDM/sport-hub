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

        public async Task<MainComment> EditComment(string message, int mainCommentId)
        {
            var foundComment = await _context.MainComments.Where(mainComment => mainComment.Id == mainCommentId).FirstOrDefaultAsync();

            foundComment.Message = message;
            foundComment.Created = DateTime.UtcNow;
          
            await _context.SaveChangesAsync();
            return foundComment;
        }

        public async Task<MainComment[]> GetSortedComments(string sortedBy, int articleId)
        {
            if (sortedBy == "popular")
            {
                var mainComments = await _context.MainComments.Include(comment => comment.User)
                                            .Where(comment => comment.ArticleId.Equals(articleId))
                                            .OrderByDescending(comment => comment.Likes)
                                            .ToArrayAsync();
                return mainComments;
            }
            else if (sortedBy == "newest")
            {
                var mainComments = await _context.MainComments.Include(comment => comment.User)
                                            .Where(comment => comment.ArticleId.Equals(articleId))
                                            .OrderByDescending(date => date.Created)
                                            .ToArrayAsync();
                return mainComments;
            }
            else 
            {
                var mainComments = await _context.MainComments.Include(comment => comment.User)
                                            .Where(comment => comment.ArticleId.Equals(articleId))
                                            .OrderBy(date => date.Created)
                                            .ToArrayAsync();
                return mainComments;
            }
        }

        public async Task<CommentUserLikeDislike>? LikeOrDislikeComment(int mainCommentId, int userId, bool isLiked)
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
    }
}
