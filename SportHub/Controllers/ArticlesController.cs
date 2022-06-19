using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportHub.Models;
using SportHub.Services.Exceptions.RootExceptions;
using SportHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IGetArticleService _articleService;

        public ArticlesController(IGetArticleService articleService)
        {
            _articleService = articleService;
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
    }
}