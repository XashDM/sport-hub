using Microsoft.AspNetCore.Mvc;
using SportHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportHub.Services.Interfaces
{
    public interface ICommentService
    {
        public void DeleteComment(int mainCommentId);
        public Task<int> GetCommentCount(int articleId);
        public Task<MainComment[]> GetComments(int articleId);
        public Task<MainComment[]> GetSortedComments(string sortedBy);
        public Task<MainComment> EditComment(string message, int mainCommentId);
    }
}
