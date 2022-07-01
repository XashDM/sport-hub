using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SportHub.Services.Interfaces;

namespace SportHub.Controllers
{
    public class CategoryController : Controller
    {

        IGetAdminArticlesService _allArticlesService;

        public CategoryController(IGetAdminArticlesService allArticlesService)
        {
            _allArticlesService = allArticlesService;
        }

        [HttpGet]
        [Route("/allcategories")]
        public async Task<IActionResult> Categories()
        {
            try
            {
                var categories = _allArticlesService.GetCategories();
                return Ok(categories);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
