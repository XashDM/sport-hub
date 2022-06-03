using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ArticlesController(IGetArticleService articleService, IImageService imageService)
        {
            _articleService = articleService;
            _imageService = imageService;
        }

        [HttpGet(nameof(GetAllCategories))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _articleService
                .GetAllCategoriesQueryable()
                .ToArrayAsync();

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
                var subcategories = await _articleService
                .GetAllSubcategoriesByCategoryIdQueryable(categoryId)
                .ToArrayAsync();

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
                var teams = await _articleService
                .GetAllTeamsByParentIdQueryable(parentId)
                .ToArrayAsync();

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

                var articles = _articleService
                    .GetAllArticlesByParentIdQueryable(articleArgs.ArticleParentId);

                var articlesPaginated = await _articleService
                    .Paginate(articles, pageArgs.PageSize, pageArgs.PageNumber)
                    .Item1
                    .ToArrayAsync();

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

                return Ok();
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

        [HttpPut(nameof(UploadPhotoOfTheDayPreview))]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPhotoOfTheDayPreview([FromForm]PhotoOfTheDayModel photo)
        {
            var link = await _imageService.UploadImageAsync(photo.imageFile);
            if (link == null)
            {
                return BadRequest("Whrong file extension!");
            }
            ImageItem imageItem = new ImageItem()
            {
                Alt = photo.Alt,
                Author = photo.Author,
                ShortDescription = photo.ShortDescription,
                PhotoTitle = photo.PhotoTitle,
                ImageLink = link
            };

            await _articleService.UploadPhotoOfTheDayPreview(imageItem);
            return Ok();
        }

        [HttpGet(nameof(GetPhotoOfTheDayPreview))]
        [AllowAnonymous]
        public async Task<IActionResult> GetPhotoOfTheDayPreview()
        {
            //image is DisplayItem
            var image = await _articleService.GetPhotoOfTheDayPreview();
            return Ok(image);
        }
    }
}