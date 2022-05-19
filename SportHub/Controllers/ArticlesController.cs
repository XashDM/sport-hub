﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet(nameof(GetAllTeamsBySubcategoryId))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTeamsBySubcategoryId(int subcategoryId)
        {
            try
            {
                var teams = await _articleService
                .GetAllTeamsBySubcategoryIdQueryable(subcategoryId)
                .ToArrayAsync();

                return Ok(teams);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(nameof(GetAllArticlesByTeamIdPaginated))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticlesByTeamIdPaginated([FromBody] ArticleArgs articleArgs)
        {
            try
            {
                var pageArgs = articleArgs.PageArgs;

                var articles = _articleService
                    .GetAllArticlesByTeamIdQueryable(articleArgs.ArticleParentId);

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

        [HttpPost(nameof(ApplyMainArticlesDisplayChanges))]  // admin-only
        [AllowAnonymous]
        public async Task<IActionResult> ApplyMainArticlesDisplayChanges(Dictionary<int, bool> articleIds)
        {
            try
            {
                await _articleService.ApplyMainArticlesDisplayChanges(articleIds);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

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
    }
}