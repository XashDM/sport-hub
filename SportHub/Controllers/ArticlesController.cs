﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain;
using SportHub.Domain.Models;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportHub.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace SportHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IGetArticleService _articleService;
        private readonly IImageService _imageService;
        private readonly ICommentService _commentService;

        public ArticlesController(IGetArticleService articleService, IImageService imageService, ICommentService commentService)
        {
            _articleService = articleService;
            _imageService = imageService;
            _commentService = commentService;
        }

        [HttpGet(nameof(GetAllCategories))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _articleService.GetAllCategoriesArrayAsync();

                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(nameof(GetAllSubcategoriesByCategoryId))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubcategoriesByCategoryId(int categoryId)
        {
            try
            {
                var subcategories = await _articleService.GetAllSubcategoriesByCategoryIdArrayAsync(categoryId);

                return Ok(subcategories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(nameof(GetAllTeamsByParentId))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTeamsByParentId(int parentId)
        {
            try
            {
                var teams = await _articleService.GetAllTeamsByParentIdArrayAsync(parentId);

                return Ok(teams);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(nameof(GetAllArticlesByParentIdPaginated))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticlesByParentIdPaginated([FromBody] ArticleArgs articleArgs)
        {
            try
            {
                var pageArgs = articleArgs.PageArgs;
                var articlesPaginated = await _articleService.GetArticlesByParentIdPaginatedArrayAsync(articleArgs.ArticleParentId, pageArgs.PageSize, pageArgs.PageNumber);

                return Ok(articlesPaginated);
            }
            catch (ArticleServiceException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(nameof(SaveMainArticles))]  // admin-only
        [AllowAnonymous]
        public async Task<IActionResult> SaveMainArticles(Dictionary<int, bool> articleIds)
        {
            try
            {
                await _articleService.SaveMainArticles(articleIds);

                return StatusCode(201);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //for admins only, returns hidden articles
        [HttpGet(nameof(GetMainArticles))]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainArticles()
        {
            try
            {
                var displayItems = await _articleService.GetMainArticles();

                return Ok(displayItems);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //for users, returns only displayed articles
        [HttpGet(nameof(GetDisplayedMainArticles))]
        [AllowAnonymous]
        public async Task<IActionResult> GetDisplayedMainArticles()
        {
            try
            {
                var articles = await _articleService.GetDisplayedMainArticles();

                return Ok(articles);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut(nameof(UploadPhotoOfTheDay))]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPhotoOfTheDay([FromForm]PhotoOfTheDayModel photo)
        {
            string link = null;
            if (photo.imageFile is not null)
            {
                link = await _imageService.UploadImageAsync(photo.imageFile);
            }

            ImageItem imageItem = new ImageItem()
            {
                Alt = photo.Alt,
                Author = photo.Author,
                ShortDescription = photo.ShortDescription,
                PhotoTitle = photo.PhotoTitle,
                ImageLink = link
            };

            var uploadResult = await _articleService.UploadPhotoOfTheDay(imageItem);
            
            if (!uploadResult)
            {
                return new BadRequestResult();
            }

            if (photo.isDisplayed is not null)
            {
                if (photo.isDisplayed == true)
                {
                    await _articleService.DisplayPhotoOfTheDay();
                }
                else
                {
                    await _articleService.HidePhotoOfTheDay();
                }
            }
            else
            {
                await _articleService.HidePhotoOfTheDay();
            }
            return Ok();
        }


        //admin only
        [HttpGet(nameof(GetPhotoOfTheDay))]
        [AllowAnonymous]
        public async Task<IActionResult> GetPhotoOfTheDay()
        {
            //image is DisplayItem
            var image = await _articleService.GetPhotoOfTheDay();
            return Ok(image);
        }

        [HttpGet(nameof(GetDisplayedPhotoOfTheDay))]
        [AllowAnonymous]
        public async Task<IActionResult> GetDisplayedPhotoOfTheDay()
        {
            //image is DisplayItem
            var image = await _articleService.GetDisplayedPhotoOfTheDay();
            return Ok(image);
        }

        [HttpPost(nameof(CreateMainComment))]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMainComment(CommentDTO comment)
        {
            var createdComment = _articleService.CreateMainComment(comment.Message, comment.ArticleId, comment.UserId);
            return Ok(createdComment);
        }

        [HttpDelete(nameof(DeleteComment))]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteComment([FromQuery] int mainCommentId)
        {
            _commentService.DeleteComment(mainCommentId);
            return Ok();
        }

        [HttpGet(nameof(GetComments))]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments([FromQuery] int articleId)
        {           
            var comments = await _commentService.GetComments(articleId);
            return Ok(comments);
        }

        [HttpGet(nameof(GetSortedComments))]
        [AllowAnonymous]
        public async Task<IActionResult> GetSortedComments([FromQuery] string sortedBy)
        {
            var sortedComments = await _commentService.GetSortedComments(sortedBy);
            return Ok(sortedComments);
        }
    }
}