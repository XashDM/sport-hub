using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SportHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/save")]
        public async Task<IActionResult> SaveItem()
        {
            return new ObjectResult("dfdf");
        }
    }
}
