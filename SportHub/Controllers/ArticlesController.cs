using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportHub.Domain.Models;
using SportHub.Models;
using SportHub.Services;
using SportHub.Services.Exceptions.RootExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportHub.Domain.Models;
using Microsoft.AspNetCore.Http;
using SportHub.Views;
using System.Linq;

namespace SportHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IGetArticleService _articleService;
        private readonly IGetAdminArticlesService _adminArticlesService;
        private readonly IImageService _imageService;
        private readonly ISearchService _searchArticles;

        public ArticlesController(IGetArticleService articleService, IGetAdminArticlesService adminArticlesService, IImageService imageService, ISearchService searchArticles)
        {
            _articleService = articleService;
            _imageService = imageService;
            _searchArticles = searchArticles;
            _adminArticlesService = adminArticlesService;
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

        [HttpPost(nameof(SaveMainArticles))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveMainArticles(MainArticlesToSaveArgs mainArticlesToSaveArgs)
        {
            try
            {
                await _articleService.SaveMainArticles(mainArticlesToSaveArgs.ArticlesDisplayValues);

                return StatusCode(201);
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

        [HttpGet(nameof(GetMainArticles))]
        [Authorize(Roles = "Admin")]
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

        [Route("/SaveNewArticle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveNewArticle([FromForm] ArticleToAdd article)
        {
            try
            {
                string link = null;
                if (article.imageFile is not null)
                {
                    link = await _imageService.UploadImageAsync(article.imageFile);
                }
                ImageItem imageItem = new ImageItem()
                {
                    Alt = article.AlternativeTextForThePicture,
                    Author = "None",
                    ShortDescription = article.Caption,
                    PhotoTitle = article.Title,
                    ImageLink = link
                };
                imageItem = await _articleService.UploadArticlePhoto(imageItem);
                if(imageItem == null)
                    return StatusCode(400, "Something went wrong");

                Article articleToSave = new Article
                {
                    ReferenceItemId = article.ReferenceItemId,
                    ImageItemId = imageItem.Id,
                    Title = article.Title,
                    Caption = article.Caption,
                    ContentText = article.ContentText,
                    IsPublished = true,
                };

                bool resulte = await _articleService.SaveArticle(articleToSave);
                if (!resulte) 
                    return StatusCode(400, "Something went wrong");

                return Ok(article);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut(nameof(UploadPhotoOfTheDay))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadPhotoOfTheDay([FromForm] PhotoOfTheDayModel photo)
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
        [Authorize(Roles = "Admin")]
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

        [HttpPost(nameof(SearchUserArticlesRange))]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUserArticlesRange([FromBody] UserArticleRange articleInfo)
        {
            var articles = _adminArticlesService.GetUserArticles(articleInfo.Category, articleInfo.Subcategory, articleInfo.Team).Result.Skip(articleInfo.startPosition).Take(articleInfo.amountOfArticles);
            return new OkObjectResult(articles);
        }

        [HttpPost(nameof(SearchArticlesRange))]
        [AllowAnonymous]
        public async Task<IActionResult> SearchArticlesRange([FromBody] ArticlesSearch articleInfo)
        {
            var articles = _searchArticles.ArticleSearchLimits(articleInfo.searchValue, articleInfo.startPosition, articleInfo.amountArticles);
            IList<ArticleForSearchResult> articleForSearchResult = new List<ArticleForSearchResult>();
            
            for (int i = 0; i < articles.Count; i++)
            {
                ArticleForSearchResult articleSearchResult = new ArticleForSearchResult();
                articleSearchResult.Id = articles[i].Id;
                articleSearchResult.ContentText = articles[i].ContentText;
                articleSearchResult.Category = _articleService.GetArticlesCategory(articles[i].Id);
                articleSearchResult.Subcategory = _articleService.GetArticlesSubcategory(articles[i].Id);
                articleSearchResult.Team = _articleService.GetArticlesTeam(articles[i].Id);
                articleForSearchResult.Add(articleSearchResult);
            }

            return new OkObjectResult(articleForSearchResult);
        }

        [HttpPost(nameof(ArticlesRange))]
        [AllowAnonymous]
        public async Task<IActionResult> ArticlesRange([FromBody] ArticlesSearch articleInfo)
        {
            var articles = _searchArticles.ArticleSearchAllTree(articleInfo.searchValue).Skip(articleInfo.startPosition).Take(articleInfo.amountArticles);
            return new OkObjectResult(articles);
        }
    }
}

        