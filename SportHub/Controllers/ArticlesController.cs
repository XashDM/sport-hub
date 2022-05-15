using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportHub.Services.Interfaces;
using System.Linq;

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

        [HttpGet(nameof(GetAllArticlesByTeamId))]
        [AllowAnonymous]
        public IActionResult GetAllArticlesByTeamId(int teamId)
        {
            var teams = _articleService
                .GetAllArticlesByTeamIdQueryable(teamId)
                .ToArray();

            return Ok(teams);
        }
    }
}
