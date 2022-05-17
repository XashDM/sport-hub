using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Services.Interfaces;
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
        public IActionResult GetAllCategories()
        {
            var categories = _articleService
                .GetAllCategoriesQueryable()
                .ToArray();

            return Ok(categories);
        }

        [HttpGet(nameof(GetAllSubcategoriesByCategoryId))]
        [AllowAnonymous]
        public IActionResult GetAllSubcategoriesByCategoryId(int categoryId)
        {
            var subcategories = _articleService
                .GetAllSubcategoriesByCategoryIdQueryable(categoryId)
                .ToArray();

            return Ok(subcategories);
        }

        [HttpGet(nameof(GetAllTeamsBySubcategoryId))]
        [AllowAnonymous]
        public IActionResult GetAllTeamsBySubcategoryId(int subcategoryId)
        {
            var teams = _articleService
                .GetAllTeamsBySubcategoryIdQueryable(subcategoryId)
                .ToArray();

            return Ok(teams);
        }

        [HttpPost(nameof(GetAllArticlesByTeamIdPaginated))]
        [AllowAnonymous]
        public IActionResult GetAllArticlesByTeamIdPaginated([FromBody] ArticleArgs articleArgs)
        {
            var pageArgs = articleArgs.PageArgs;

            var articles = _articleService
                .GetAllArticlesByTeamIdQueryable(articleArgs.ArticleParentId);

            var articlesPaginated = _articleService
                .Paginate(articles, pageArgs.PageSize, pageArgs.PageNumber)
                .Item1
                .ToArray();

            return Ok(articlesPaginated);
        }

        [HttpPost(nameof(ApplyMainArticlesDisplayChanges))]
        [AllowAnonymous]
        public async Task<IActionResult> ApplyMainArticlesDisplayChanges(Dictionary<int, bool> articleIds)
        {
            await _articleService.ApplyMainArticlesDisplayChanges(articleIds);

            return Ok();
        }

        [HttpGet(nameof(GetMainArticles))]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainArticles()
        {
            var displayItems =  await _articleService.GetMainArticles();

            return Ok(displayItems);
        }
    }
}